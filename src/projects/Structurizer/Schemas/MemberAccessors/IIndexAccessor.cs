using System;
using System.Collections.Generic;

namespace Structurizer.Schemas.MemberAccessors
{
    public interface IIndexAccessor
    {
        string Path { get; }
        Type DataType { get; }
        DataTypeCode DataTypeCode { get; }
        bool IsEnumerable { get; }
        bool IsElement { get; }

        IList<IStructureIndexValue> GetValues<T>(T item) where T : class;
    }
}