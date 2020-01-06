namespace Structurizer
{
    public enum DataTypeCode
    {
        Unknown,
        IntegerNumber,
        UnsignedIntegerNumber,
        FractalNumber,
        Bool,
        DateTime,
        Guid,
        String,
        Enum
    }

    public static class DataTypeCodeExtensions
    {
        public static bool IsNumeric(this DataTypeCode code) =>
            code == DataTypeCode.IntegerNumber ||
            code == DataTypeCode.FractalNumber ||
            code == DataTypeCode.UnsignedIntegerNumber;
    }
}