using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Moq;
using NUnit.Framework;
using Structurizer.Schemas.MemberAccessors;

namespace Structurizer.UnitTests.Schemas
{
    [TestFixture]
    public class StructureSchemaFactoryTests : UnitTestBase
    {
        private readonly IStructureTypeFactory _structureTypeFactory = new StructureTypeFactory();
        private readonly IStructureSchemaFactory _structureSchemaFactory = new StructureSchemaFactory();

        private IStructureType GetStructureTypeFor<T>()
            where T : class
        {
            return _structureTypeFactory.CreateFor(typeof(T));
        }

        [Test]
        public void CreateSchema_WhenNestedType_SchemaNameReflectsTypeName()
        {
            const string expectedName = "WithIdAndIndexableFirstLevelMembers";
            var structureType = GetStructureTypeFor<WithIdAndIndexableFirstLevelMembers>();

            var schema = _structureSchemaFactory.CreateSchema(structureType);

            Assert.AreEqual(expectedName, schema.Name);
        }

        [Test]
        public void CreateSchema_WhenSecondLevelIndexablePropertiesExists_IndexAccessorsAreCreated()
        {
            var structureType = GetStructureTypeFor<WithFirstSecondAndThirdLevelMembers>();

            var schema = _structureSchemaFactory.CreateSchema(structureType);

            var hasSecondLevelAccessors = schema.IndexAccessors.Any(iac => HasLevel(iac, 1));
            Assert.IsTrue(hasSecondLevelAccessors);
        }

        [Test]
        public void CreateSchema_WhenSecondLevelIndexablePropertiesExists_PathReflectsHierarchy()
        {
            var structureType = GetStructureTypeFor<WithFirstSecondAndThirdLevelMembers>();

            var schema = _structureSchemaFactory.CreateSchema(structureType);

            var secondLevelItems = schema.IndexAccessors.Where(iac => HasLevel(iac, 1));
            Assert.IsTrue(secondLevelItems.All(iac => iac.Path.StartsWith("SecondLevelItem.")));
        }

        [Test]
        public void CreateSchema_WhenThirdLevelIndexablePropertiesExists_IndexAccessorsAreCreated()
        {
            var structureType = GetStructureTypeFor<WithFirstSecondAndThirdLevelMembers>();

            var schema = _structureSchemaFactory.CreateSchema(structureType);

            var hasThirdLevelAccessors = schema.IndexAccessors.Any(iac => HasLevel(iac, 2));
            Assert.IsTrue(hasThirdLevelAccessors);
        }

        [Test]
        public void CreateSchema_WhenThirdLevelIndexablePropertiesExists_PathReflectsHierarchy()
        {
            var structureType = GetStructureTypeFor<WithFirstSecondAndThirdLevelMembers>();

            var schema = _structureSchemaFactory.CreateSchema(structureType);

            var thirdLevelItems = schema.IndexAccessors.Where(iac => HasLevel(iac, 2));
            Assert.IsTrue(thirdLevelItems.All(iac => iac.Path.StartsWith("SecondLevelItem.ThirdLevelItem.")));
        }

        [Test]
        public void CreateSchema_WhenThirdLevelIndexableEnumerablePropertiesExists_IndexAccessorsAreCreated()
        {
            var structureType = GetStructureTypeFor<WithFirstSecondAndThirdLevelMembers>();

            var schema = _structureSchemaFactory.CreateSchema(structureType);

            var hasThirdLevelAccessors = schema.IndexAccessors.Any(iac => HasLevel(iac, 2) && iac.Path == "SecondLevelItem.ThirdLevelItem.Numbers");
            Assert.IsTrue(hasThirdLevelAccessors);
        }

        [Test]
        public void CreateSchema_WhenItemHasIndexableFirstLevelProperties_IndexAccessorsAreExtracted()
        {
            var structureType = GetStructureTypeFor<WithIdAndIndexableFirstLevelMembers>();

            var schema = _structureSchemaFactory.CreateSchema(structureType);

            CollectionAssert.IsNotEmpty(schema.IndexAccessors);
        }

        [Test]
        public void CreateSchema_WhenGuidItemHasNoIndexableFirstLevelProperties_ThrowsMissingIndexableMembersException()
        {
            var structureType = new Mock<IStructureType>();
            structureType.Setup(s => s.Name).Returns("TmpType");

            var ex = Assert.Throws<StructurizerException>(
                () => _structureSchemaFactory.CreateSchema(structureType.Object));

            var expectedMessage = string.Format(StructurizerExceptionMessages.AutoSchemaBuilder_MissingIndexableMembers, "TmpType");
            Assert.AreEqual(expectedMessage, ex.Message);
        }

        [Test]
        public void CreateSchema_WhenItemHasNoIndexableFirstLevelProperties_ThrowsMissingIndexableMembersException()
        {
            var structureType = new Mock<IStructureType>();
            structureType.Setup(s => s.Name).Returns("TmpType");

            var ex = Assert.Throws<StructurizerException>(
                () => _structureSchemaFactory.CreateSchema(structureType.Object));

            var expectedMessage = string.Format(StructurizerExceptionMessages.AutoSchemaBuilder_MissingIndexableMembers, "TmpType");
            Assert.AreEqual(expectedMessage, ex.Message);
        }

        [Test]
        public void CreateSchema_WhenByteMember_IndexAccessorIsCreatedForByteMember()
        {
            var structureType = GetStructureTypeFor<WithByte>();

            var schema = _structureSchemaFactory.CreateSchema(structureType);

            var byteIac = schema.IndexAccessors.SingleOrDefault(iac => iac.Path == "Byte");
            Assert.IsNotNull(byteIac);
            Assert.IsTrue(byteIac.DataType == typeof(byte));
        }

        [Test]
        public void CreateSchema_WhenNullableByteMember_IndexAccessorIsCreatedForByteMember()
        {
            var structureType = GetStructureTypeFor<WithNullableByte>();

            var schema = _structureSchemaFactory.CreateSchema(structureType);

            var byteIac = schema.IndexAccessors.SingleOrDefault(iac => iac.Path == "Byte");
            Assert.IsNotNull(byteIac);
            Assert.IsTrue(byteIac.DataType == typeof(byte?));
        }

        [Test]
        public void CreateSchema_WhenItemWithByteArray_NoIndexShouldBeCreatedForByteArray()
        {
            var structureType = GetStructureTypeFor<WithBytes>();

            var schema = _structureSchemaFactory.CreateSchema(structureType);

            Assert.AreEqual(1, schema.IndexAccessors.Count);
            Assert.IsTrue(schema.IndexAccessors[0].Path.StartsWith("DummyMember"));
        }

        [Test]
        public void CreateSchema_WhenClassContainsStructMember_StructMemberIsRepresentedInSchema()
        {
            var structureType = GetStructureTypeFor<WithStruct>();

            var schema = _structureSchemaFactory.CreateSchema(structureType);

            Assert.AreEqual(1, schema.IndexAccessors.Count);
            Assert.AreEqual("Content", schema.IndexAccessors[0].Path);
            Assert.AreEqual(typeof(MyText), schema.IndexAccessors[0].DataType);
        }

        private static bool HasLevel(IIndexAccessor iac, int level)
        {
            var count = iac.Path.Count(ch => ch == '.');

            return count == level;
        }

        private class WithByte
        {
            public byte Byte { get; set; }
        }

        private class WithNullableByte
        {
            public byte? Byte { get; set; }
        }

        private class WithIdAndIndexableFirstLevelMembers
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        private class WithFirstSecondAndThirdLevelMembers
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public SecondLevelItem SecondLevelItem { get; set; }
        }

        private class SecondLevelItem
        {
            public string Street { get; set; }
            public string Zip { get; set; }
            public string City { get; set; }
            public ThirdLevelItem ThirdLevelItem { get; set; }
        }

        private class ThirdLevelItem
        {
            public int AreaCode { get; set; }
            public int Number { get; set; }
            public int[] Numbers { get; set; }
        }

        private class WithBytes
        {
            public int DummyMember { get; set; }
            public byte[] Bytes1 { get; set; }
            public IEnumerable<byte> Bytes2 { get; set; }
            public IList<byte> Bytes3 { get; set; }
            public List<byte> Bytes4 { get; set; }
            public ICollection<byte> Bytes5 { get; set; }
            public Collection<byte> Bytes6 { get; set; }
        }

        private class WithStruct
        {
            public MyText Content { get; set; }
        }

        private struct MyText
        {
            private readonly string _value;

            public MyText(string value)
            {
                _value = value;
            }

            public static MyText Parse(string value)
            {
                return value == null ? null : new MyText(value);
            }

            public static implicit operator MyText(string value)
            {
                return new MyText(value);
            }

            public static implicit operator string(MyText item)
            {
                return item._value;
            }

            public override string ToString()
            {
                return _value;
            }
        }
    }
}