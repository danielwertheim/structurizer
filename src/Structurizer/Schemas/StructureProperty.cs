using System;
using Structurizer.Extensions;

namespace Structurizer.Schemas
{
    public class StructureProperty : IStructureProperty
    {
        private readonly DynamicGetter _getter;
        private readonly DynamicSetter _setter;

        public string Name { get; }
        public string Path { get; }
        public Type DataType { get; }
        public IStructureProperty Parent { get; }
        public bool IsRootMember { get; }
        public bool IsEnumerable { get; }
        public bool IsElement { get; }
        public Type ElementDataType { get; }
        public bool IsReadOnly { get; }

        public StructureProperty(StructurePropertyInfo info, DynamicGetter getter, DynamicSetter setter = null)
        {
            _getter = getter;
            _setter = setter;

            Parent = info.Parent;
            Name = info.Name;
            DataType = info.DataType;
            IsRootMember = info.Parent == null;
            IsReadOnly = _setter == null;

            var isSimpleOrValueType = DataType.IsSimpleType() || DataType.IsValueType;
            IsEnumerable = !isSimpleOrValueType && DataType.IsEnumerableType();
            IsElement = Parent != null && (Parent.IsElement || Parent.IsEnumerable);
            ElementDataType = IsEnumerable ? DataType.GetEnumerableElementType() : null;
            Path = PropertyPathBuilder.BuildPath(this);
        }

        public virtual object GetValue(object item) => _getter.GetValue(item);

        public virtual void SetValue(object target, object value)
        {
            if (IsReadOnly)
                throw new StructurizerException(string.Format(StructurizerExceptionMessages.StructureProperty_Setter_IsReadOnly, Path));

            _setter.SetValue(target, value);
        }
    }
}