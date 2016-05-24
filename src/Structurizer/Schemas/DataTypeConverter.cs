using System;
using System.Linq;
using Structurizer.Extensions;

namespace Structurizer.Schemas
{
    public class DataTypeConverter : IDataTypeConverter
    {
        public static readonly string[] DefaultTextDataTypeConventions = new[] { "Text", "Content", "Description" };

        public Func<string, bool> MemberNameIsForTextType { get; set; }

        public DataTypeConverter()
        {
            MemberNameIsForTextType = OnMemberNameIsForTextType;
        }

        protected virtual bool OnMemberNameIsForTextType(string memberName)
        {
            return DefaultTextDataTypeConventions.Any(convention => memberName.EndsWith(convention, StringComparison.OrdinalIgnoreCase));
        }

        public virtual DataTypeCode Convert(IStructureProperty property)
        {
            return Convert(property.ElementDataType ?? property.DataType, property.Name);
        }

        public virtual DataTypeCode Convert(Type dataType, string memberName)
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
            {
                return MemberNameIsForTextType(memberName)
                    ? DataTypeCode.Text
                    : DataTypeCode.String;
            }

            return dataType.IsAnyEnumType()
                ? DataTypeCode.Enum
                : DataTypeCode.Unknown;
        }
    }
}