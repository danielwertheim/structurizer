using System.Reflection;

namespace Structurizer
{
    public interface IStructurePropertyFactory
    {
        IStructureProperty CreateRootPropertyFrom(PropertyInfo propertyInfo);
        IStructureProperty CreateChildPropertyFrom(IStructureProperty parent, PropertyInfo propertyInfo);
    }
}