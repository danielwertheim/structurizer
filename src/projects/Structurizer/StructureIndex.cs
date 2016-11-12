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

            var valueIsOkType = value is string || value is ValueType;
            if (value != null && !valueIsOkType)
                throw new ArgumentException(StructurizerExceptionMessages.StructureIndex_ValueArgument_IncorrectType);

            Name = name;
            Path = path;
            Value = value;
            DataType = dataType;
            DataTypeCode = dataTypeCode;
        }
    }
}