using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Structurizer;

namespace UnitTests.Schemas
{
	[TestClass]
	public class DataTypeConverterTests : UnitTests
	{
	    private IDataTypeConverter _converter;

        public DataTypeConverterTests()
        {
            _converter = new DataTypeConverter();
        }

        private IStructureProperty CreateProperty(Type type, string name = null)
        {
            var property = new Mock<IStructureProperty>();
            property.SetupGet(f => f.Name).Returns(name ?? "Foo");
            property.SetupGet(f => f.DataType).Returns(type);

            return property.Object;
        }

        [TestMethod]
        [DataRow(typeof(ushort))]
        [DataRow(typeof(ushort?))]
        [DataRow(typeof(uint))]
        [DataRow(typeof(uint?))]
        [DataRow(typeof(ulong))]
        [DataRow(typeof(ulong?))]
        public void Convert_TypeIsUnsignedIntegerFamily_ReturnsUnsignedIntegerNumber(Type type)
        {
            Assert.AreEqual(DataTypeCode.UnsignedIntegerNumber, _converter.Convert(CreateProperty(type)));
        }

		[TestMethod]
        [DataRow(typeof(short))]
        [DataRow(typeof(short?))]
		[DataRow(typeof(int))]
		[DataRow(typeof(int?))]
        [DataRow(typeof(long))]
        [DataRow(typeof(long?))]
		public void Convert_TypeIsIntegerFamily_ReturnsIntegerNumber(Type type)
		{
			Assert.AreEqual(DataTypeCode.IntegerNumber, _converter.Convert(CreateProperty(type)));
		}

		[TestMethod]
		[DataRow(typeof(Single))]
		[DataRow(typeof(Single?))]
		[DataRow(typeof(double))]
		[DataRow(typeof(double?))]
		[DataRow(typeof(decimal))]
		[DataRow(typeof(decimal?))]
		[DataRow(typeof(float))]
		[DataRow(typeof(float?))]
		public void Convert_TypeIsFractalFamily_ReturnsFractalNumber(Type type)
		{
			Assert.AreEqual(DataTypeCode.FractalNumber, _converter.Convert(CreateProperty(type)));
		}

		[TestMethod]
		[DataRow(typeof(bool))]
		[DataRow(typeof(bool?))]
		public void Convert_TypeIsBool_ReturnsBool(Type type)
		{
			Assert.AreEqual(DataTypeCode.Bool, _converter.Convert(CreateProperty(type)));
		}

		[TestMethod]
		[DataRow(typeof(DateTime))]
		[DataRow(typeof(DateTime?))]
		public void Convert_TypeIsDateTime_ReturnsDateTime(Type type)
		{
			Assert.AreEqual(DataTypeCode.DateTime, _converter.Convert(CreateProperty(type)));
		}

		[TestMethod]
		[DataRow(typeof(Guid))]
		[DataRow(typeof(Guid?))]
		public void Convert_TypeIsGuid_ReturnsGuid(Type type)
		{
			Assert.AreEqual(DataTypeCode.Guid, _converter.Convert(CreateProperty(type)));
		}

		[TestMethod]
		[DataRow(typeof(string))]
		public void Convert_TypeIsString_ReturnsString(Type type)
		{
			Assert.AreEqual(DataTypeCode.String, _converter.Convert(CreateProperty(type)));
		}

		[TestMethod]
		[DataRow(typeof(DataTypeCode))]
		[DataRow(typeof(DataTypeCode?))]
		public void Convert_TypeIsEnum_ReturnsEnum(Type type)
		{
			Assert.AreEqual(DataTypeCode.Enum, _converter.Convert(CreateProperty(type)));
		}
	}
}