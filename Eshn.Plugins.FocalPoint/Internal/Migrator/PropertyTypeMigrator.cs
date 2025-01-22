using System.Linq;
using EPiServer.DataAbstraction;
using EPiServer.DataAbstraction.Migration;
using EPiServer.ServiceLocation;
using Eshn.Plugins.FocalPoint.SpecializedProperties;

namespace Eshn.Plugins.FocalPoint.Internal.Migrator;

public class PropertyTypeMigratorStep : MigrationStep
{
    public override void AddChanges()
    {
        ChangePropertyType();
    }

    private void ChangePropertyType()
    {
        var propertyTypeRepository = ServiceLocator.Current.GetInstance<IPropertyDefinitionTypeRepository>();;
        var focalPointPropertyDef = propertyTypeRepository.List().FirstOrDefault(e =>
            e.TypeName == "ImageResizer.Plugins.EPiFocalPoint.SpecializedProperties.PropertyFocalPoint");
        if (focalPointPropertyDef != null)
        {
            focalPointPropertyDef = focalPointPropertyDef.CreateWritableClone();
            focalPointPropertyDef.TypeName = typeof(PropertyFocalPoint).FullName;
            focalPointPropertyDef.AssemblyName = typeof(PropertyFocalPoint).Assembly.GetName().Name;
            propertyTypeRepository.Save(focalPointPropertyDef);
        }
    }
}