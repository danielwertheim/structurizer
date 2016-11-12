using System;

namespace Structurizer
{
    public interface IStructureIndex
    {
        string Name { get; }
        string Path { get; }
        object Value { get; }
        Type DataType { get; }
        DataTypeCode DataTypeCode { get; }
    }
}