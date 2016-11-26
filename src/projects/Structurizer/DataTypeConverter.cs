using System;
using Structurizer.Extensions;

namespace Structurizer
{
    public class DataTypeConverter : IDataTypeConverter
    {
        public DataTypeCode Convert(IStructureProperty property)
        {
            return Convert(property.ElementDataType ?? property.DataType);
        }

        public DataTypeCode Convert(Type dataType)
        {
            if (dataType.IsAnySignedIntegerNumberType())
                return DataTypeCode.IntegerNumber;

            if (dataType.IsAnyUnsignedType())
                return DataTypeCode.UnsignedIntegerNumber;

            if (dataType.IsAnyFractalNumberType())
                return DataTypeCode.FractalNumber;

            if (dataType.IsAnyBoolType())
                return DataTypeCode.Bool;

            if (dataType.IsAnyDateTimeType())
                return DataTypeCode.DateTime;

            if (dataType.IsAnyGuidType())
                return DataTypeCode.Guid;

            if (dataType.IsStringType())
                return DataTypeCode.String;

            return dataType.IsAnyEnumType()
                ? DataTypeCode.Enum
                : DataTypeCode.Unknown;
        }
    }
}