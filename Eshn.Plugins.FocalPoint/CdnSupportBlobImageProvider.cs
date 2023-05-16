using System.Text.RegularExpressions;
using Baaijte.Optimizely.ImageSharp.Web.Providers;
using Baaijte.Optimizely.ImageSharp.Web.Resolvers;
using EPiServer;
using EPiServer.Core;
using EPiServer.Editor;
using EPiServer.Web;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using SixLabors.ImageSharp.Web;
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
               || path.Contains("/siteassets/", StringComparison.OrdinalIgnoreCase);
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