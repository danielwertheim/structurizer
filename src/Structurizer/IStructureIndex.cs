using System;

namespace Structurizer
{
    public interface IStructureIndex
    {
        string Path { get; }
        INodeValue Node { get; }
        Type DataType { get; }
        DataTypeCode DataTypeCode { get; }
    }
}