using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Structurizer.Schemas;

namespace Structurizer.UnitTests.Schemas
{
    public class StructureSchemaTests : UnitTestBase
    {
        [Test]
        public void Should_initialize_type_and_name()
        {
            var structureType = new StructureType(typeof(Foo));

            var schema = new StructureSchema(structureType);

            schema.StructureType.Type.Should().Be(typeof(Foo));
            schema.Name.Should().Be(typeof(Foo).Name);
        }

        [Test]
        public void Should_assign_empty_index_accessors_When_not_passed()
        {
            var structureType = new StructureType(typeof(Foo));

            var schema = new StructureSchema(structureType);

            schema.IndexAccessors.Should().BeEmpty();
        }

        [Test]
        public void Should_assign_index_accessors_When_present()
        {
            var structureType = new StructureType(typeof(Foo));

            var schema = new StructureSchema(structureType, new List<IIndexAccessor> { Mock.Of<IIndexAccessor>() });

            schema.IndexAccessors.Should().HaveCount(1);
        }

        private class Foo
        {
            public int SomeInt { get; set; }
        }
    }
}