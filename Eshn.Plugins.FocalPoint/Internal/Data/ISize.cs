namespace Eshn.Plugins.FocalPoint.Internal.Data {
	public interface ISize {
		int Width { get; }
		int Height { get; }
		bool IsEmpty { get; }
		bool IsValid { get; }
	}
}