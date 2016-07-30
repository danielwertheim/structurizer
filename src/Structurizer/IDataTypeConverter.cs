using System;

namespace Structurizer
{
    public interface IDataTypeConverter
    {
        DataTypeCode Convert(IStructureProperty property);
        DataTypeCode Convert(Type dataType, string memberName);
    }
}