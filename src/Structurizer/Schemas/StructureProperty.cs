using System;
using Structurizer.Extensions;

namespace Structurizer.Schemas
{
    public class StructureProperty : IStructureProperty
    {
        private readonly DynamicGetter _getter;

        public string Name { get; }
        public string Path { get; }
        public Type DataType { get; }
        public IStructureProperty Parent { get; }
        public bool IsRootMember { get; }
        public bool IsEnumerable { get; }
        public bool IsElement { get; }
        public Type ElementDataType { get; }

        public StructureProperty(StructurePropertyInfo info, DynamicGetter getter)
        {
            _getter = getter;

            Parent = info.Parent;
            Name = info.Name;
            DataType = info.DataType;
            IsRootMember = info.Parent == null;

            var isSimpleOrValueType = DataType.IsSimpleType() || DataType.IsValueType;
            IsEnumerable = !isSimpleOrValueType && DataType.IsEnumerableType();
            IsElement = Parent != null && (Parent.IsElement || Parent.IsEnumerable);
            ElementDataType = IsEnumerable ? DataType.GetEnumerableElementType() : null;
            Path = PropertyPathBuilder.BuildPath(this);
        }

        public virtual object GetValue(object item) => _getter.GetValue(item);
    }
}