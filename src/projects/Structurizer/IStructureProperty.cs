using System;

namespace Structurizer
{
    public interface IStructureProperty
    {
        string Name { get; }
        string Path { get; }
        Type DataType { get; }
        IStructureProperty Parent { get; }
        bool IsRootMember { get; }
        bool IsEnumerable { get; }
        bool IsElement { get; }
        Type ElementDataType { get; }
        Attribute[] Attributes { get; }

        object GetValue(object item);
    }
}