using System;
using EnsureThat;

namespace Structurizer
{
    public class StructureIndex : IStructureIndex
    {
        public string Path { get; }
        public INodeValue Node { get; }
        public Type DataType { get; }
        public DataTypeCode DataTypeCode { get; }

        public StructureIndex(string path, INodeValue nodeValue, Type dataType, DataTypeCode dataTypeCode)
        {
            Ensure.That(path, nameof(path)).IsNotNullOrWhiteSpace();
            Ensure.That(dataType, nameof(path)).IsNotNull();

            var valueIsOkType = nodeValue.Value is string || nodeValue.Value is ValueType;
            if (nodeValue != null && !valueIsOkType)
                throw new ArgumentException(StructurizerExceptionMessages.StructureIndex_ValueArgument_IncorrectType);

            Path = path;
            Node = nodeValue;
            DataType = dataType;
            DataTypeCode = dataTypeCode;
        }
    }
}