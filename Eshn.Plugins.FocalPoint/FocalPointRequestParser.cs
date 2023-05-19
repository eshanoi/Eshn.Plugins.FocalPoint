using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using SixLabors.ImageSharp.Web.Commands;

namespace Eshn.Plugins.FocalPoint;

public class FocalPointRequestParser : IRequestParser
{
    private IRequestParser _defaultRequestParser;

    private static Dictionary<string, string> _queryParamsMapping = new()
    {
        { "mode", "rmode" },
        { "pad", "BoxPad" }
    };

    public FocalPointRequestParser(IRequestParser defaultRequestParser)
    {
        _defaultRequestParser = defaultRequestParser;
    }

    public CommandCollection ParseRequestCommands(HttpContext context)
    {
        var currentContent = ServiceLocator.Current.GetInstance<IContentRouteHelper>().Content;
        var imageOption = ServiceLocator.Current.GetInstance<IOptions<EpiImageSharpMiddlewareOptions>>();
        if (context.Request.Query.Count == 0 || currentContent is not IFocalPointData focalPointData)
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
            string newKey = _queryParamsMapping.TryGetValue(pair.Key, out var temp) ? temp : pair.Key;
            // Use the indexer for both set and query. This replaces any previously parsed values.
            transformed[newKey] = pair.Value[^1];
        }

        int width = 0, height = 0;
        if (ignoreZoomIn && focalPointData.OriginalWidth.HasValue && focalPointData.OriginalHeight.HasValue
                         && (transformed.TryGetValue("width", out var widthStr) && int.TryParse(widthStr, out width))
            || (transformed.TryGetValue("height", out var heightStr) && int.TryParse(heightStr, out height)))
        {
            if (width != 0)
            {
                width = focalPointData.OriginalWidth.HasValue && width > focalPointData.OriginalWidth
                    ? focalPointData.OriginalWidth!.Value
                    : width;
                transformed["width"] = width.ToString();
            }

            if (height != 0)
            {
                height = focalPointData.OriginalHeight.HasValue && height > focalPointData.OriginalHeight
                    ? focalPointData.OriginalHeight!.Value
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
}