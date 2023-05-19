using SixLabors.ImageSharp.Web.Middleware;

namespace Eshn.Plugins.FocalPoint;

public class EpiImageSharpMiddlewareOptions : ImageSharpMiddlewareOptions
{
    /// <summary>
    /// Disable resize when image width or height is less than expect width or height
    /// </summary>
    public bool IgnoreZoomIn { get; set; }
}