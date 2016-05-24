using System;

namespace Structurizer.Schemas
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
        bool IsReadOnly { get; }
        object GetValue(object item);
        void SetValue(object target, object value);
    }
}