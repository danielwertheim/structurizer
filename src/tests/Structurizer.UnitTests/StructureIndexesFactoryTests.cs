using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Structurizer.UnitTests
{
    [TestFixture]
    public class StructureIndexesFactoryTests : UnitTestBase
    {
        [Test]
        public void GetIndexes_WhenItemHasGuidId_ReturnsId()
        {
            var value = Guid.Parse("1F0E8C1D-7AF5-418F-A6F6-A40B7F31CB00");
            var item = new WithGuid { GuidValue = value };
            var schemaStub = StructureSchemaTestFactory.CreateRealFrom<WithGuid>();

            var factory = new StructureIndexesFactory();
            var indexes = factory.CreateIndexes(schemaStub, item).ToList();

            Assert.AreEqual(value, indexes.Single(i => i.Path == "GuidValue").Value);
        }

        [Test]
        public void GetIndexes_WhenItemHasNulledGuidId_ReturnsNoIndex()
        {
            var item = new WithNullableGuid { GuidValue = null };
            var schemaStub = StructureSchemaTestFactory.CreateRealFrom<WithNullableGuid>();

            var factory = new StructureIndexesFactory();
            var indexes = factory.CreateIndexes(schemaStub, item).ToList();

            Assert.AreEqual(0, indexes.Count);
        }

        [Test]
        public void GetIndexes_WhenNullableGuidIdHasValue_ReturnsId()
        {
            var value = Guid.Parse("1F0E8C1D-7AF5-418F-A6F6-A40B7F31CB00");
            var item = new WithNullableGuid { GuidValue = value };
            var schemaStub = StructureSchemaTestFactory.CreateRealFrom<WithNullableGuid>();

            var factory = new StructureIndexesFactory();
            var indexes = factory.CreateIndexes(schemaStub, item).ToList();

            Assert.AreEqual(value, indexes.Single(i => i.Path == "GuidValue").Value);
        }

        [Test]
        public void GetIndexes_WhenItemWithNullString_ReturnsNoIndex()
        {
            var item = new WithNoArray { StringValue = null };
            var schemaStub = StructureSchemaTestFactory.CreateRealFrom<WithNoArray>();

            var factory = new StructureIndexesFactory();
            var indexes = factory.CreateIndexes(schemaStub, item).ToList();

            Assert.IsNull(indexes.SingleOrDefault(i => i.Path == "StringValue"));
        }

        [Test]
        public void GetIndexes_WhenItemWithAssignedString_ReturnsIndexWithStringValue()
        {
            var item = new WithNoArray { StringValue = "A" };
            var schemaStub = StructureSchemaTestFactory.CreateRealFrom<WithNoArray>();

            var factory = new StructureIndexesFactory();
            var indexes = factory.CreateIndexes(schemaStub, item).ToList();

            Assert.AreEqual("A", indexes.Single(i => i.Path == "StringValue").Value);
        }

        [Test]
        public void GetIndexes_WhenItemWithAssignedInt_ReturnsIndexWithIntValue()
        {
            var item = new WithNoArray { IntValue = 42 };
            var schemaStub = StructureSchemaTestFactory.CreateRealFrom<WithNoArray>();

            var factory = new StructureIndexesFactory();
            var indexes = factory.CreateIndexes(schemaStub, item).ToList();

            Assert.AreEqual(42, indexes.Single(i => i.Path == "IntValue").Value);
        }

        [Test]
        public void GetIndexes_WhenItemWithEnumerableWithOneNullInt_ReturnsNullIndex()
        {
            var item = new WithArray { NullableIntValues = new int?[] { null } };
            var schemaStub = StructureSchemaTestFactory.CreateRealFrom<WithArray>();

            var factory = new StructureIndexesFactory();
            var indexes = factory.CreateIndexes(schemaStub, item).ToList();

            Assert.IsNull(indexes.SingleOrDefault(i => i.Path == "NullableIntValues"));
        }

        [Test]
        public void GetIndexes_WhenItemWithEnumerableWithOneNullString_ReturnsNullIndex()
        {
            var item = new WithArray { StringValues = new string[] { null } };
            var schemaStub = StructureSchemaTestFactory.CreateRealFrom<WithArray>();

            var factory = new StructureIndexesFactory();
            var indexes = factory.CreateIndexes(schemaStub, item).ToList();

            Assert.IsNull(indexes.SingleOrDefault(i => i.Path == "StringValues"));
        }

        [Test]
        public void GetIndexes_WhenItemWithEnumerableWithOneString_ReturnsIndexWithString()
        {
            var item = new WithArray { StringValues = new[] { "A" } };
            var schemaStub = StructureSchemaTestFactory.CreateRealFrom<WithArray>();

            var factory = new StructureIndexesFactory();
            var indexes = factory.CreateIndexes(schemaStub, item).ToList();

            Assert.AreEqual("A", indexes.Single(i => i.Path == "StringValues[0]").Value);
        }

        [Test]
        public void GetIndexes_WhenItemWithEnumerableWithOneString_ReturnsIndexWithDataTypeOfStringElement()
        {
            var item = new WithArray { StringValues = new[] { "A" } };
            var schemaStub = StructureSchemaTestFactory.CreateRealFrom<WithArray>();

            var factory = new StructureIndexesFactory();
            var indexes = factory.CreateIndexes(schemaStub, item).ToList();

            Assert.AreEqual(DataTypeCode.String, indexes.Single(i => i.Path == "StringValues[0]").DataTypeCode);
        }

        [Test]
        public void GetIndexes_WhenItemWithComplexEnumerable_ReturnsIndexWithDataTypeOfStringElement()
        {
            var item = new WithComplexArray { Items = new[] { new Complex { Name = "Foo", Value = 42 } } };
            var schemaStub = StructureSchemaTestFactory.CreateRealFrom<WithComplexArray>();

            var factory = new StructureIndexesFactory();
            var indexes = factory.CreateIndexes(schemaStub, item).ToList();

            Assert.AreEqual(DataTypeCode.String, indexes.Single(i => i.Path == "Items[0].Name").DataTypeCode);
            Assert.AreEqual(DataTypeCode.IntegerNumber, indexes.Single(i => i.Path == "Items[0].Value").DataTypeCode);
        }

        [Test]
        public void GetIndexes_WhenItemWithEnumerableWithOneInt_ReturnsIndexWithInt()
        {
            var item = new WithArray { IntValues = new[] { 42 } };
            var schemaStub = StructureSchemaTestFactory.CreateRealFrom<WithArray>();

            var factory = new StructureIndexesFactory();
            var indexes = factory.CreateIndexes(schemaStub, item).ToList();

            Assert.AreEqual(42, indexes.Single(i => i.Path == "IntValues[0]").Value);
        }

        [Test]
        public void GetIndexes_WhenItemWithEnumerableWithTwoDifferentStrings_ReturnsTwoStringIndexes()
        {
            var item = new WithArray { StringValues = new[] { "A", "B" } };
            var schemaStub = StructureSchemaTestFactory.CreateRealFrom<WithArray>();

            var factory = new StructureIndexesFactory();
            var indexes = factory.CreateIndexes(schemaStub, item).ToList();

            Assert.AreEqual("A", indexes[0].Value);
            Assert.AreEqual("B", indexes[1].Value);
        }

        [Test]
        public void GetIndexes_WhenItemWithEnumerableWithTwoDifferentInts_ReturnsTwoIntIndexes()
        {
            var item = new WithArray { IntValues = new[] { 42, 43 } };
            var schemaStub = StructureSchemaTestFactory.CreateRealFrom<WithArray>();

            var factory = new StructureIndexesFactory();
            var indexes = factory.CreateIndexes(schemaStub, item).ToList();

            Assert.AreEqual(42, indexes[0].Value);
            Assert.AreEqual(43, indexes[1].Value);
        }

        [Test]
        public void GetIndexes_WhenItemWithEnumerableWithTwoEqualElements_ReturnsTwoStringIndexes()
        {
            var item = new WithArray { StringValues = new[] { "A", "A" } };
            var schemaStub = StructureSchemaTestFactory.CreateRealFrom<WithArray>();

            var factory = new StructureIndexesFactory();
            var indexes = factory.CreateIndexes(schemaStub, item).ToList();

            Assert.AreEqual("A", indexes[0].Value);
            Assert.AreEqual("A", indexes[1].Value);
        }

        [Test]
        public void GetIndexes_WhenItemWithEnumerableWithTwoEqualElements_ReturnsTwoIntIndexes()
        {
            var item = new WithArray { IntValues = new[] { 42, 42 } };
            var schemaStub = StructureSchemaTestFactory.CreateRealFrom<WithArray>();

            var factory = new StructureIndexesFactory();
            var indexes = factory.CreateIndexes(schemaStub, item).ToList();

            Assert.AreEqual(42, indexes[0].Value);
            Assert.AreEqual(42, indexes[1].Value);
        }

        [Test]
        public void GetIndexes_WhenItemWithArraysBeingNull_ReturnesNoIndexes()
        {
            var item = new WithArray
            {
                IntValues = null,
                NullableIntValues = null,
                StringValues = null
            };

            var schemaStub = StructureSchemaTestFactory.CreateRealFrom<WithArray>();

            var factory = new StructureIndexesFactory();
            var indexes = factory.CreateIndexes(schemaStub, item).ToList();

            Assert.IsEmpty(indexes);
        }

        [Test]
        public void GetIndexes_WhenArrayOfComplexWithChildBeingNull_ReturnesNoIndexes()
        {
            var item = new WithComplexArray
            {
                Items = new[] { new Complex { Name = null, Value = 42 } }
            };

            var schemaStub = StructureSchemaTestFactory.CreateRealFrom<WithComplexArray>();

            var factory = new StructureIndexesFactory();
            var indexes = factory.CreateIndexes(schemaStub, item).ToList();

            indexes.Count.Should().Be(1);
            indexes[0].Value.Should().Be(42);
        }

        private class WithGuid
        {
            public Guid GuidValue { get; set; }
        }

        private class WithNullableGuid
        {
            public Guid? GuidValue { get; set; }
        }

        private class WithNoArray
        {
            public string StringValue { get; set; }
            public int IntValue { get; set; }
        }

        private class WithArray
        {
            public string[] StringValues { get; set; }
            public int[] IntValues { get; set; }
            public int?[] NullableIntValues { get; set; }
        }

        private class WithComplexArray
        {
            public Complex[] Items { get; set; }
        }

        private class Complex
        {
            public string Name { get; set; }
            public int Value { get; set; }
        }
    }
}