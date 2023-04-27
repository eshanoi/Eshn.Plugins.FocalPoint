using System.Drawing;

namespace Eshn.Plugins.FocalPoint.Internal.Data {
	internal class InvalidSize : ISize {
		private readonly Size size = Size.Empty;
		public int Width => size.Width;
		public int Height => size.Height;
		public bool IsEmpty => size.IsEmpty;
		public bool IsValid => false;
	}
}