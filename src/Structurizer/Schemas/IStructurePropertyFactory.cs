using System.Reflection;

namespace Structurizer.Schemas
{
    public interface IStructurePropertyFactory
    {
        IStructureProperty CreateRootPropertyFrom(PropertyInfo propertyInfo);
        IStructureProperty CreateChildPropertyFrom(IStructureProperty parent, PropertyInfo propertyInfo);
    }
}