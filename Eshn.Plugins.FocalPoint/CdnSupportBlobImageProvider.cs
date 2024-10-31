using Baaijte.Optimizely.ImageSharp.Web.Resolvers;
using EPiServer.Core;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp.Web.Providers;
using SixLabors.ImageSharp.Web.Resolvers;

namespace Eshn.Plugins.FocalPoint;

public class CdnSupportBlobImageProvider : IImageProvider
{
    private readonly IContentRouteHelper _contentRouteHelper;

    public CdnSupportBlobImageProvider(IContentRouteHelper contentRouteHelper)
    {
        _contentRouteHelper = contentRouteHelper;
        Match = IsMatch;
    }

    private static bool IsMatch(HttpContext context)
    {
        var path = context.Request.Path.ToString();

        return path.Contains("/contentassets/", StringComparison.OrdinalIgnoreCase)
               || path.Contains("/globalassets/", StringComparison.OrdinalIgnoreCase)
               || path.Contains("/siteassets/", StringComparison.OrdinalIgnoreCase)
               || path.Contains("/SysSiteAssets/", StringComparison.OrdinalIgnoreCase);
    }

    public bool IsValidRequest(HttpContext context) =>
        _contentRouteHelper.Content is MediaData { BinaryData: not null };

    public Task<IImageResolver> GetAsync(HttpContext context)
    {
        return _contentRouteHelper.Content is MediaData { BinaryData: not null } media
            ? Task.FromResult((IImageResolver)new BlobImageResolver(media))
            : Task.FromResult((IImageResolver)null);
    }
        

    public ProcessingBehavior ProcessingBehavior { get; }
    public Func<HttpContext, bool> Match { get; set; }
}