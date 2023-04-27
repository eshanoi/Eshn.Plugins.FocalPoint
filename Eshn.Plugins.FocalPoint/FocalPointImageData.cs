using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using Eshn.Plugins.FocalPoint.SpecializedProperties;

namespace Eshn.Plugins.FocalPoint
{
    public abstract class FocalPointImageData : ImageData, IFocalPointData
    {
        [BackingType(typeof(PropertyFocalPoint))]
        public virtual SpecializedProperties.FocalPoint FocalPoint { get; set; }

        [ScaffoldColumn(false)] public virtual int? OriginalWidth { get; set; }

        [ScaffoldColumn(false)] public virtual int? OriginalHeight { get; set; }
    }
}