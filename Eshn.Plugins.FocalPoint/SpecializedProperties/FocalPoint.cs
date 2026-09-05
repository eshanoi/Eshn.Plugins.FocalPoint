
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Eshn.Plugins.FocalPoint.SpecializedProperties {
	public class FocalPoint {
		[JsonPropertyName("x")]
		public double X { get; set; }
		[JsonPropertyName("y")]
		public double Y { get; set; }
		public override string ToString() {
			return JsonSerializer.Serialize(this);
		}
	}
}