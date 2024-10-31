using EPiServer;
using EPiServer.DataAccess;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Eshn.Plugins.FocalPoint.Internal.Data;
using Eshn.Plugins.FocalPoint.Internal.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using SixLabors.ImageSharp.Web.Commands;

namespace Eshn.Plugins.FocalPoint;

public class FocalPointRequestParser : IRequestParser
{
    private readonly IContentRepository _contentRepository;
    private readonly ILogger<IRequestParser> _logger;
    private static readonly Dictionary<string, string> QueryParamsMapping = new()
    {
        { "mode", "rmode" },
        { "pad", "BoxPad" }
    };
    public FocalPointRequestParser(ILogger<IRequestParser> logger, IContentRepository contentRepository)
    {
        _logger = logger;
        _contentRepository = contentRepository;
    }

    public CommandCollection ParseRequestCommands(HttpContext context)
    {
        var currentContent = ServiceLocator.Current.GetInstance<IContentRouteHelper>().Content;
        var imageOption = ServiceLocator.Current.GetInstance<IOptions<EpiImageSharpMiddlewareOptions>>();
        if (context.Request.Query.Count == 0)
        {
            // We return new to ensure the collection is still mutable via events.
            return new();
        }

        bool ignoreZoomIn = imageOption.Value.IgnoreZoomIn;
        Dictionary<string, StringValues> parsed =
            QueryHelpers.ParseQuery(context.Request.QueryString.ToUriComponent().Replace("?", "&"));
        CommandCollection transformed = new();
        foreach (KeyValuePair<string, StringValues> pair in parsed)
        {
            string newKey = QueryParamsMapping.TryGetValue(pair.Key, out var temp) ? temp : pair.Key;
            // Use the indexer for both set and query. This replaces any previously parsed values.
            transformed[newKey] = pair.Value[^1];
        }

        if (currentContent is not IFocalPointData focalPointData)
        {
            return transformed;
        }
        int width = 0, height = 0;
        bool hasWidth = transformed.TryGetValue("width", out var widthStr) && int.TryParse(widthStr, out width);
        bool hasHeight = transformed.TryGetValue("height", out var heightStr) && int.TryParse(heightStr, out height);
        int? originalWidth = focalPointData.OriginalWidth, originalHeight = focalPointData.OriginalHeight;
        ISize? size;
        if (!originalWidth.HasValue && !originalHeight.HasValue &&
            (size = CalculateSize(focalPointData)) != null)
        {
            originalWidth = size.Width;
            originalHeight = size.Height;
        }

        if (ignoreZoomIn && originalWidth.HasValue && originalHeight.HasValue
            && (hasWidth || hasHeight))
        {
            if (width != 0)
            {
                width = width > originalWidth
                    ? originalWidth.Value
                    : width;
                transformed["width"] = width.ToString();
            }

            if (height != 0)
            {
                height = height >originalHeight
                    ? originalHeight.Value
                    : height;
                transformed["height"] = height.ToString();
            }
        }

        if (transformed.TryGetValue("rxy", out _) is false && focalPointData.FocalPoint is not null)
        {
            transformed["rxy"] = $"{focalPointData.FocalPoint.X / 100},{focalPointData.FocalPoint.Y / 100}";
        }

        return transformed;
    }

    private ISize? CalculateSize(IFocalPointData focalPointData)
    {
        if (focalPointData is { BinaryData: not null })
        {
            using var stream = focalPointData.BinaryData.OpenRead();
            try
            {
                var size = ImageDimensionService.GetDimensions(stream);
                if (size.IsValid)
                {
                    _logger.LogInformation($"Update {focalPointData.ContentLink} original size");
                    var content = _contentRepository.Get<IFocalPointData>(focalPointData.ContentLink).CreateWritableClone() as IFocalPointData;
                    content!.OriginalHeight = -1;
                    _contentRepository.Save(content, SaveAction.Publish, AccessLevel.NoAccess);
                    return size;
                }
            }
            catch
            {
                // ignored
            }
        }

        return null;
    }
}