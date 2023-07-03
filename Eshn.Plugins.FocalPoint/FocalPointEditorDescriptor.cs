using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using Eshn.Plugins.FocalPoint.SpecializedProperties;

namespace Eshn.Plugins.FocalPoint
{
    [EditorDescriptorRegistration(TargetType = typeof(SpecializedProperties.FocalPoint))]
    public class FocalPointEditorDescriptor : EditorDescriptor
    {
        public FocalPointEditorDescriptor()
        {
            ClientEditingClass = "focal-point/editor";
        }

        public override void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            ClientEditingClass = "focal-point/editor";
            base.ModifyMetadata(metadata, attributes);
        }
    }
}