using Baaijte.Optimizely.ImageSharp.Web;
using Baaijte.Optimizely.ImageSharp.Web.Caching;
using EPiServer.ServiceLocation;
using EPiServer.Shell.Modules;
using Eshn.Plugins.FocalPoint;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp.Web.Commands;
using SixLabors.ImageSharp.Web.DependencyInjection;
using SixLabors.ImageSharp.Web.Middleware;
using SixLabors.ImageSharp.Web.Providers;

namespace Microsoft.Extensions.DependencyInjection;

public static class FocalPointExtension
{
    public static IImageSharpBuilder AddEPiFocalPointImageSharp(
        this IServiceCollection services,
        Action<EpiImageSharpMiddlewareOptions> setupAction)
    {
        services.Configure(setupAction);
        IImageSharpBuilder builder = services.AddImageSharp(option =>
        {
            var eOption = ServiceLocator.Current.GetInstance<IOptions<EpiImageSharpMiddlewareOptions>>().Value;
            option.BrowserMaxAge = eOption.BrowserMaxAge;
            option.CacheHashLength = eOption.CacheHashLength;
            option.CacheMaxAge = eOption.CacheMaxAge;
            option.UseInvariantParsingCulture = eOption.UseInvariantParsingCulture;
            option.HMACSecretKey = eOption.HMACSecretKey;
            
        });
        services.Intercept<IRequestParser>((_, service) => new FocalPointRequestParser(service));
        services.Configure<ProtectedModuleOptions>(x =>
        {
            if (!x.Items.Any(moduleDetails => moduleDetails.Name.Equals("focal-point")))
            {
                x.Items.Add(new ModuleDetails
                {
                    Name = "focal-point"
                });
            }
        });
        return builder;
    }
    public static IImageSharpBuilder AddEPiFocalPointImageSharp(
       this IServiceCollection services) => AddEPiFocalPointImageSharp(services, _ => { });

    public static void UseEpiFocalPointImageSharp(this IApplicationBuilder app) => app.UseBaaijteOptimizelyImageSharp();
}