using System.Reflection;

namespace Structurizer.Schemas
{
    public class StructurePropertyFactory : IStructurePropertyFactory
    {
        public virtual IStructureProperty CreateRootPropertyFrom(PropertyInfo propertyInfo)
        {
            return new StructureProperty(
                ConvertInfo(propertyInfo),
                DynamicPropertyFactory.GetterFor(propertyInfo),
                DynamicPropertyFactory.SetterFor(propertyInfo));
        }

        public virtual IStructureProperty CreateChildPropertyFrom(IStructureProperty parent, PropertyInfo propertyInfo)
        {
            return new StructureProperty(
                ConvertInfo(propertyInfo, parent),
                DynamicPropertyFactory.GetterFor(propertyInfo),
                DynamicPropertyFactory.SetterFor(propertyInfo));
        }

        protected virtual StructurePropertyInfo ConvertInfo(PropertyInfo propertyInfo, IStructureProperty parent = null)
        {
            return new StructurePropertyInfo(
                propertyInfo.Name,
                propertyInfo.PropertyType,
                parent);
        }
    }
}