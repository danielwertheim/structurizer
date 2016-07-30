using System;

namespace Structurizer
{
    public interface IDataTypeConverter
    {
        Func<string, bool> MemberNameIsForTextType { get; set; }
        DataTypeCode Convert(IStructureProperty property);
        DataTypeCode Convert(Type dataType, string memberName);
    }
}