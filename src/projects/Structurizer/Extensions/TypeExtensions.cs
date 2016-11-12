using System;
using System.Collections;
using System.Collections.Generic;

namespace Structurizer.Extensions
{
    internal static class TypeExtensions
    {
        private static readonly Type EnumerableType = typeof(IEnumerable);
        private static readonly Type DictionaryType = typeof(IDictionary);
        private static readonly Type DictionaryOfTType = typeof(IDictionary<,>);
        private static readonly Type KeyValuePairType = typeof(KeyValuePair<,>);
        private static readonly Type EnumType = typeof(Enum);

        private static readonly Type StringType = typeof(string);
        private static readonly Type DateTimeType = typeof(DateTime);
        private static readonly Type BoolType = typeof(bool);
        private static readonly Type GuidType = typeof(Guid);
        private static readonly Type CharType = typeof(char);

        private static readonly Type ByteType = typeof(byte);
        private static readonly Type ShortType = typeof(short);
        private static readonly Type IntType = typeof(int);
        private static readonly Type LongType = typeof(long);

        private static readonly Type SingleType = typeof(Single);
        private static readonly Type FloatType = typeof(float);
        private static readonly Type DecimalType = typeof(decimal);
        private static readonly Type DoubleType = typeof(double);

        private static readonly Type NullableType = typeof(Nullable<>);

        private static readonly Type NullableDateTimeType = typeof(DateTime?);
        private static readonly Type NullableGuidType = typeof(Guid?);
        private static readonly Type NullableBoolType = typeof(bool?);
        private static readonly Type NullableCharType = typeof(Char?);

        private static readonly Type NullableByteType = typeof(byte?);
        private static readonly Type NullableShortType = typeof(short?);
        private static readonly Type NullableIntType = typeof(int?);
        private static readonly Type NullableLongType = typeof(long?);

        private static readonly Type NullableSingleType = typeof(Single?);
        private static readonly Type NullableFloatType = typeof(float?);
        private static readonly Type NullableDecimalType = typeof(decimal?);
        private static readonly Type NullableDoubleType = typeof(double?);

        private static readonly HashSet<Type> ExtraPrimitiveTypes = new HashSet<Type> { typeof(string), typeof(Guid), typeof(DateTime), typeof(Decimal) };
        private static readonly HashSet<Type> ExtraPrimitiveNullableTypes = new HashSet<Type> { typeof(Guid?), typeof(DateTime?), typeof(Decimal?) };
        private static readonly HashSet<Type> UnsignedTypes = new HashSet<Type> { typeof(ushort), typeof(uint), typeof(ulong) };
        private static readonly HashSet<Type> NullableUnsignedTypes = new HashSet<Type> { typeof(ushort?), typeof(uint?), typeof(ulong?) };

        internal static bool IsSimpleType(this Type type)
        {
            return (type.IsGenericType == false && type.IsValueType) || type.IsPrimitive || type.IsEnum || ExtraPrimitiveTypes.Contains(type) || type.IsNullablePrimitiveType();
        }

        internal static bool IsKeyValuePairType(this Type type)
        {
            return type.IsGenericType && type.IsValueType && type.GetGenericTypeDefinition() == KeyValuePairType;
        }

        internal static bool IsNumericType(this Type type)
        {
            return
                IsAnyIntegerNumberType(type) ||
                IsAnyFractalNumberType(type);
        }

        internal static bool IsAnyIntegerNumberType(this Type type)
        {
            return type.IsAnySignedIntegerNumberType() || type.IsAnyUnsignedType();
        }

        internal static bool IsAnySignedIntegerNumberType(this Type type)
        {
            return type.IsAnyIntType()
                || type.IsAnyLongType()
                || type.IsAnyShortType()
                || type.IsAnyByteType();
        }

        internal static bool IsAnyFractalNumberType(this Type type)
        {
            return type.IsAnyDoubleType()
                || type.IsAnyDecimalType()
                || type.IsAnySingleType()
                || type.IsAnyFloatType();
        }

        internal static bool IsEnumerableType(this Type type)
        {
            return type != StringType
                && type.IsValueType == false
                && type.IsPrimitive == false
                && EnumerableType.IsAssignableFrom(type);
        }

        internal static bool IsEnumerableBytesType(this Type type)
        {
            if (!IsEnumerableType(type))
                return false;

            var elementType = GetEnumerableElementType(type);

            return elementType.IsByteType() || elementType.IsNullableByteType();
        }

        internal static Type GetEnumerableElementType(this Type type)
        {
            var elementType = (type.IsGenericType ? ExtractEnumerableGenericType(type) : type.GetElementType());
            if (elementType != null)
                return elementType;

            if (type.BaseType.IsEnumerableType())
                elementType = type.BaseType.GetEnumerableElementType();

            return elementType;
        }

        private static Type ExtractEnumerableGenericType(Type type)
        {
            var generics = type.GetGenericArguments();

            if (generics.Length == 1)
                return generics[0];

            if (generics.Length == 2 && (DictionaryType.IsAssignableFrom(type) || type.GetGenericTypeDefinition() == DictionaryOfTType))
                return KeyValuePairType.MakeGenericType(generics[0], generics[1]);

            throw new StructurizerException(StructurizerExceptionMessages.TypeExtensions_ExtractEnumerableGenericType);
        }

        internal static bool IsStringType(this Type t)
        {
            return t == StringType;
        }

        internal static bool IsDateTimeType(this Type t)
        {
            return t == DateTimeType;
        }

        internal static bool IsAnyDateTimeType(this Type t)
        {
            return IsDateTimeType(t) || IsNullableDateTimeType(t);
        }

        internal static bool IsBoolType(this Type t)
        {
            return t == BoolType;
        }

        internal static bool IsAnyBoolType(this Type t)
        {
            return IsBoolType(t) || IsNullableBoolType(t);
        }

        internal static bool IsDecimalType(this Type t)
        {
            return t == DecimalType;
        }

        internal static bool IsAnyDecimalType(this Type t)
        {
            return IsDecimalType(t) || IsNullableDecimalType(t);
        }

        internal static bool IsSingleType(this Type t)
        {
            return t == SingleType;
        }

        internal static bool IsAnySingleType(this Type t)
        {
            return IsSingleType(t) || IsNullableSingleType(t);
        }

        internal static bool IsFloatType(this Type t)
        {
            return t == FloatType;
        }

        internal static bool IsAnyFloatType(this Type t)
        {
            return IsFloatType(t) || IsNullableFloatType(t);
        }

        internal static bool IsDoubleType(this Type t)
        {
            return t == DoubleType;
        }

        internal static bool IsAnyDoubleType(this Type t)
        {
            return IsDoubleType(t) || IsNullableDoubleType(t);
        }

        internal static bool IsLongType(this Type t)
        {
            return t == LongType;
        }

        internal static bool IsAnyLongType(this Type t)
        {
            return IsLongType(t) || IsNullableLongType(t);
        }

        internal static bool IsGuidType(this Type t)
        {
            return t == GuidType;
        }

        internal static bool IsAnyGuidType(this Type t)
        {
            return IsGuidType(t) || IsNullableGuidType(t);
        }

        internal static bool IsIntType(this Type t)
        {
            return t == IntType;
        }

        internal static bool IsAnyIntType(this Type t)
        {
            return IsIntType(t) || IsNullableIntType(t);
        }

        internal static bool IsByteType(this Type t)
        {
            return t == ByteType;
        }

        internal static bool IsAnyByteType(this Type t)
        {
            return IsByteType(t) || IsNullableByteType(t);
        }

        internal static bool IsShortType(this Type t)
        {
            return t == ShortType;
        }

        internal static bool IsAnyShortType(this Type t)
        {
            return IsShortType(t) || IsNullableShortType(t);
        }

        internal static bool IsCharType(this Type t)
        {
            return t == CharType;
        }

        internal static bool IsAnyCharType(this Type t)
        {
            return IsCharType(t) || IsNullableCharType(t);
        }

        internal static bool IsEnumType(this Type t)
        {
            return (t.BaseType == EnumType) || t.IsEnum;
        }

        internal static bool IsAnyEnumType(this Type t)
        {
            return IsEnumType(t) || IsNullableEnumType(t);
        }

        internal static bool IsNullablePrimitiveType(this Type t)
        {
            return ExtraPrimitiveNullableTypes.Contains(t) || (t.IsValueType && t.IsGenericType && t.GetGenericTypeDefinition() == NullableType && t.GetGenericArguments()[0].IsPrimitive);
        }

        internal static bool IsNullableDateTimeType(this Type t)
        {
            return t == NullableDateTimeType;
        }

        internal static bool IsNullableDecimalType(this Type t)
        {
            return t == NullableDecimalType;
        }

        internal static bool IsNullableSingleType(this Type t)
        {
            return t == NullableSingleType;
        }

        internal static bool IsNullableFloatType(this Type t)
        {
            return t == NullableFloatType;
        }

        internal static bool IsNullableDoubleType(this Type t)
        {
            return t == NullableDoubleType;
        }

        internal static bool IsNullableBoolType(this Type t)
        {
            return t == NullableBoolType;
        }

        internal static bool IsNullableGuidType(this Type t)
        {
            return t == NullableGuidType;
        }

        internal static bool IsNullableShortType(this Type t)
        {
            return t == NullableShortType;
        }

        internal static bool IsNullableIntType(this Type t)
        {
            return t == NullableIntType;
        }

        internal static bool IsNullableByteType(this Type t)
        {
            return t == NullableByteType;
        }

        internal static bool IsNullableLongType(this Type t)
        {
            return t == NullableLongType;
        }

        internal static bool IsNullableCharType(this Type t)
        {
            return t == NullableCharType;
        }

        internal static bool IsNullableEnumType(this Type t)
        {
            if (t.IsGenericType && t.GetGenericTypeDefinition() == NullableType)
            {
                t = Nullable.GetUnderlyingType(t);
                return t.IsEnumType();
            }

            return false;
        }

        internal static bool IsAnyUnsignedType(this Type t)
        {
            return t.IsUnsignedType() || t.IsNullableUnsignedType();
        }

        internal static bool IsUnsignedType(this Type t)
        {
            return t.IsValueType && UnsignedTypes.Contains(t);
        }

        internal static bool IsNullableUnsignedType(this Type t)
        {
            return t.IsValueType && NullableUnsignedTypes.Contains(t);
        }
    }
}