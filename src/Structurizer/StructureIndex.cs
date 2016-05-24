using System;
using EnsureThat;

namespace Structurizer
{
    [Serializable]
    public class StructureIndex : IStructureIndex
    {
        public string Path { get; }
        public object Value { get; }
        public Type DataType { get; }
        public DataTypeCode DataTypeCode { get; }

        public StructureIndex(string path, object value, Type dataType, DataTypeCode dataTypeCode)
        {
            Ensure.That(path, nameof(path)).IsNotNullOrWhiteSpace();
            Ensure.That(dataType, nameof(path)).IsNotNull();

            var valueIsOkType = value is string || value is ValueType;
            if (value != null && !valueIsOkType)
                throw new ArgumentException(StructurizerExceptionMessages.StructureIndex_ValueArgument_IncorrectType);

            Path = path;
            Value = value;
            DataType = dataType;
            DataTypeCode = dataTypeCode;
        }
    }
}