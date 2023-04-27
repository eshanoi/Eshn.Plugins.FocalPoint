using EPiServer.Core;
using EPiServer.Data.Entity;
using Eshn.Plugins.FocalPoint.SpecializedProperties;

namespace Eshn.Plugins.FocalPoint {
	public interface IFocalPointData : IContentImage, IReadOnly {
		SpecializedProperties.FocalPoint FocalPoint { get; set; }
		int? OriginalWidth { get; set; }
		int? OriginalHeight { get; set; }
	}
}