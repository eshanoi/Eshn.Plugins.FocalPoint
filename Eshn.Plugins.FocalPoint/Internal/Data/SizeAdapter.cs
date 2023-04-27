using System.Drawing;

namespace Eshn.Plugins.FocalPoint.Internal.Data {
	internal class SizeAdapter : ISize {
		private readonly Size size;
		public SizeAdapter(Size size) {
			this.size = size;
		}
		public int Width => size.Width;
		public int Height => size.Height;
		public bool IsEmpty => size.IsEmpty;
		public bool IsValid => true;
	}
}