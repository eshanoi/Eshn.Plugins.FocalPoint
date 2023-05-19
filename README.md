### License

- Eshn.Plugins.FocalPoint is licensed under the [Apache License, Version 2.0](https://opensource.org/licenses/Apache-2.0)
- This package is migrated and customized from <https://github.com/defsteph/EPiFocalPoint> and we use [Baaijte.Optimizely.ImageSharp.Web](https://github.com/vnbaaij/Baaijte.Optimizely.ImageSharp.Web) for caching and resizing image.

### Installation

Eshn.Plugins.FocalPoiunt is installed via [Optimizely nuget feed](https://nuget.optimizely.com/package/?id=Eshn.Plugins.FocalPoint)

#### Package Manager

```bash
PM > Install-Package Eshn.Plugins.FocalPoint -Version VERSION_NUMBER
```

#### .NET CLI

```bash
dotnet add package Eshn.Plugins.FocalPoint --version VERSION_NAME
```

### Setup and configuration

Once installed you will need to add the following code to `ConfigurationServices` and `Configure` in your `Start.cs` file.

```csharp
public void ConfigureServices(IServiceCollection services) {
    // Add the default service and options.
    services.AddEPiFocalPointImageSharp();
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {

    // Add the image processing middleware.
    app.UseEpiFocalPointImageSharp();
}
```

**_DO NOT_** add `SixLabors.ImageSharp.Web` and `Baaijte.Optimizely.ImageSharp.Web` settings.

### Usage

Create or make sure your Image Media ContentTypes inherits from `Eshn.Plugins.FocalPoint.FocalPointImageData` or implements `Eshn.Plugins.FocalPoint.IFocalPointImageData`. Remember to decorate the `FocalPoint` property with `[BackingType(typeof(PropertyFocalPoint))]` if you want to implement `Eshn.Plugins.FocalPoint.IFocalPointImageData` manually.

This is example code:

```csharp
public abstract class ImageFile : ImageData, IFocalPointData
{
    [BackingType(typeof(PropertyFocalPoint))]
    public virtual FocalPoint? FocalPoint { get; set; }

    [ScaffoldColumn(false)]
    public virtual int? OriginalWidth { get; set; }

    [ScaffoldColumn(false)]
    public virtual int? OriginalHeight { get; set; }
}

```

Disable resize when image width or height is less than expect width or height

```csharp
var builder = services.AddEPiFocalPointImageSharp(options =>
{
    options.IgnoreZoomIn = true;
});
```
#### Additional localizations

Embedded localizations are provided for Swedish and English. Should you need to localize in other languages, you can do so by adding XML translations thusly:

```xml
<contenttypes>
    <imagedata>
        <properties>
            <focalpoint>
                <caption>Focal point</caption>
                <help>The point in the image, where the focus should be, automatically cropped images will be calculated based on this point.</help>
            </focalpoint>
            <originalheight>
                <caption>Height</caption>
                <help>The image height in pixels.</help>
            </originalheight>
            <originalwidth>
                <caption>Width</caption>
                <help>The image width in pixels.</help>
            </originalwidth>
        </properties>
    </imagedata>
</contenttypes>
```
