using System;
using System.Text.Json;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.PlugIn;

namespace Eshn.Plugins.FocalPoint.SpecializedProperties
{
    [PropertyDefinitionType(Description = "A property to edit the focal point of an image",
        DisplayName = "Focal Point")]
    public class PropertyFocalPoint : PropertyLongString
    {
        public override PropertyDataType Type => PropertyDataType.Json;
        public override Type PropertyValueType => typeof(FocalPoint);

        public override object? Value
        {
            get
            {
                var value = base.Value as string;
                return value == null ? null : JsonSerializer.Deserialize<FocalPoint>(value);
            }
            set => base.Value = value is FocalPoint ? JsonSerializer.Serialize(value) : value;
        }

        public override object SaveData(PropertyDataCollection properties)
        {
            return LongString;
        }
    }
}