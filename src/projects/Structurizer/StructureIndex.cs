using System;
using EnsureThat;

namespace Structurizer
{
    public class StructureIndex : IStructureIndex
    {
        public string Name { get; }
        public string Path { get; }
        public object Value { get; }
        public Type DataType { get; }
        public DataTypeCode DataTypeCode { get; }

        public StructureIndex(string name, string path, object value, Type dataType, DataTypeCode dataTypeCode)
        {
            EnsureArg.IsNotNullOrWhiteSpace(name, nameof(name));
            EnsureArg.IsNotNullOrWhiteSpace(path, nameof(path));
            EnsureArg.IsNotNull(dataType, nameof(dataType));

            Name = name;
            Path = path;
            Value = value;
            DataType = dataType;
            DataTypeCode = dataTypeCode;
        }
    }
}