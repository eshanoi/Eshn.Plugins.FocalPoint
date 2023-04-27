using Newtonsoft.Json;

namespace Eshn.Plugins.FocalPoint.SpecializedProperties {
	public class FocalPoint {
		[JsonProperty("x")]
		public double X { get; set; }
		[JsonProperty("y")]
		public double Y { get; set; }
		public override string ToString() {
			return JsonConvert.SerializeObject(this);
		}
	}
}