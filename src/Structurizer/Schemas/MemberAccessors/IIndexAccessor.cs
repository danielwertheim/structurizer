using System.Collections.Generic;

namespace Structurizer.Schemas.MemberAccessors
{
    public interface IIndexAccessor : IMemberAccessor
    {
        DataTypeCode DataTypeCode { get; }
        bool IsEnumerable { get; }
        bool IsElement { get; }

        IList<INodeValue> GetValues<T>(T item) where T : class;
        void SetValue<T>(T item, object value) where T : class;
    }
}