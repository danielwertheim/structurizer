using System;
using System.Linq;
using System.Reflection;

namespace Structurizer.Schemas
{
    public class StructurePropertyFactory : IStructurePropertyFactory
    {
        public virtual IStructureProperty CreateRootPropertyFrom(PropertyInfo propertyInfo) => new StructureProperty(
            ConvertInfo(propertyInfo),
            DynamicPropertyFactory.GetterFor(propertyInfo));

        public virtual IStructureProperty CreateChildPropertyFrom(IStructureProperty parent, PropertyInfo propertyInfo) => new StructureProperty(
            ConvertInfo(propertyInfo, parent),
            DynamicPropertyFactory.GetterFor(propertyInfo));

        private static StructurePropertyInfo ConvertInfo(PropertyInfo propertyInfo, IStructureProperty parent = null) => new StructurePropertyInfo(
            propertyInfo.Name,
            propertyInfo.PropertyType,
            propertyInfo.GetCustomAttributes(true).OfType<Attribute>().ToArray(),
            parent);
    }
}