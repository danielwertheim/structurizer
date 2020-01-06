using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Structurizer;

namespace UnitTests.Schemas
{
    [TestClass]
    public class StructureSchemaFactoryTests : UnitTests
    {
        private static IStructureSchemaFactory CreateSut(IDataTypeConverter converter = null)
            => new StructureSchemaFactory(converter ?? Mock.Of<IDataTypeConverter>());

        //Dear lord, give me strength to instead fully mock the hiearchy of... I tried, but I gave up
        private IStructureType GetStructureTypeFor<T>() where T : class => StructureTypeTestFactory.CreateFor<T>();

        [TestMethod]
        public void Should_create_schema_for_passed_type()
        {
            var structureType = GetStructureTypeFor<WithTwoProperties>();

            var schema = CreateSut().CreateSchema(structureType);

            Assert.AreEqual(structureType.Type, schema.StructureType.Type);
        }

        [TestMethod]
        public void Should_throw_When_structure_type_contains_no_index_accessors()
        {
            var structureType = GetStructureTypeFor<WithNoProperties>();

            Action action = () => CreateSut().CreateSchema(structureType);

            action
                .Should().Throw<StructurizerException>()
                .WithMessage(string.Format(StructurizerExceptionMessages.AutoSchemaBuilder_MissingIndexableMembers, structureType.Name));
        }

        [TestMethod]
        public void Should_create_index_accessors_for_each_indexable_property()
        {
            var structureType = GetStructureTypeFor<WithTwoProperties>();

            var schema = CreateSut().CreateSchema(structureType);
            schema.IndexAccessors.Should().HaveCount(2);
        }

        [TestMethod]
        public void Should_use_data_type_converter_When_creating_index_accessors()
        {
            var dataTypeConverterMock = new Mock<IDataTypeConverter>();
            var structureType = GetStructureTypeFor<WithTwoProperties>();

            CreateSut(dataTypeConverterMock.Object).CreateSchema(structureType);

            dataTypeConverterMock.Verify(m => m.Convert(It.IsAny<IStructureProperty>()), Times.Exactly(2));
        }

        private class WithNoProperties { }

        private class WithTwoProperties
        {
            public int SomeIntProp { get; set; }
            public string SomeStringProp { get; set; }
        }
    }
}