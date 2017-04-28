using System;
using Structurizer.Extensions;

namespace Structurizer.Schemas
{
    public class StructureProperty : IStructureProperty
    {
        private readonly StructurePropertyInfo _info;
        private readonly DynamicGetter _getter;

        public string Name => _info.Name;
        public string Path { get; }
        public Type DataType => _info.DataType;
        public IStructureProperty Parent => _info.Parent;
        public bool IsRootMember { get; }
        public bool IsEnumerable { get; }
        public bool IsElement { get; }
        public Type ElementDataType { get; }
        public object[] Attributes => _info.Attributes;

        public StructureProperty(StructurePropertyInfo info, DynamicGetter getter)
        {
            _info = info;
            _getter = getter;

            IsRootMember = info.Parent == null;
            IsEnumerable = !DataType.IsSimpleType() && DataType.IsEnumerableType();
            IsElement = Parent != null && (Parent.IsElement || Parent.IsEnumerable);
            ElementDataType = IsEnumerable ? DataType.GetEnumerableElementType() : null;
            Path = PropertyPathBuilder.BuildPath(this);
        }

        public object GetValue(object item) => _getter(item);
    }
}