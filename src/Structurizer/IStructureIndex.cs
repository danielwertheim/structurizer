using System;

namespace Structurizer
{
    public interface IStructureIndex
    {
        string Path { get; }
        object Value { get; }
        Type DataType { get; }
        DataTypeCode DataTypeCode { get; }
    }
}