using Baaijte.Optimizely.ImageSharp.Web;
using Baaijte.Optimizely.ImageSharp.Web.Caching;
using EPiServer.Shell.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SixLabors.ImageSharp.Web.Commands;
using SixLabors.ImageSharp.Web.DependencyInjection;
using SixLabors.ImageSharp.Web.Middleware;
using SixLabors.ImageSharp.Web.Providers;

namespace Eshn.Plugins.FocalPoint;

public static class FocalPointExtension
{
    public static IImageSharpBuilder AddEPiFocalPointImageSharp(
        this IServiceCollection services,
        Action<ImageSharpMiddlewareOptions> setupAction)
    {
        IImageSharpBuilder builder = services.AddImageSharp(setupAction);
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
}