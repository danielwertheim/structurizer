using System;
using FluentAssertions;
using NUnit.Framework;

namespace Structurizer.UnitTests.StructureBuilderTests
{
    [TestFixture]
    public class StructureBuilderNullablesTests : StructureBuilderBaseTests
    {
        [Test]
        public void CreateStructure_WhenItemWithNullablesHasValues_IndexesAreCreated()
        {
            Builder = StructureBuilder.Create(c => c.Register<TestItemWithNullables>());
            var item = TestItemWithNullables.CreatePopulated();

            var structure = Builder.CreateStructure(item);

            Assert.AreEqual(2, structure.Indexes.Count);

            Assert.AreEqual("NullableInt", structure.Indexes[0].Path);
            Assert.AreEqual(typeof(int?), structure.Indexes[0].DataType);
            Assert.AreEqual(item.NullableInt, structure.Indexes[0].Node);

            Assert.AreEqual("NullableGuid", structure.Indexes[1].Path);
            Assert.AreEqual(typeof(Guid?), structure.Indexes[1].DataType);
            Assert.AreEqual(item.NullableGuid, structure.Indexes[1].Node);
        }

        [Test]
        public void CreateStructure_WhenItemWithNullablesHasNullValues_NoIndexesAreCreated()
        {
            Builder = StructureBuilder.Create(c => c.Register<TestItemWithNullables>());
            var item = TestItemWithNullables.CreateWithNullValues();

            var structure = Builder.CreateStructure(item);

            structure.Indexes.Should().BeEmpty();
        }

        [Test]
        public void CreateStructure_WhenChildItemWithInheritedNullablesHasValues_IndexesAreCreated()
        {
            Builder = StructureBuilder.Create(c => c.Register<ChildWithNullables>());
            var item = ChildWithNullables.CreatePopulated();

            var structure = Builder.CreateStructure(item);

            Assert.AreEqual(2, structure.Indexes.Count);

            Assert.AreEqual("NullableInt", structure.Indexes[0].Path);
            Assert.AreEqual(typeof(int?), structure.Indexes[0].DataType);
            Assert.AreEqual(item.NullableInt, structure.Indexes[0].Node);

            Assert.AreEqual("NullableGuid", structure.Indexes[1].Path);
            Assert.AreEqual(typeof(Guid?), structure.Indexes[1].DataType);
            Assert.AreEqual(item.NullableGuid, structure.Indexes[1].Node);
        }

        [Test]
        public void CreateStructure_WhenChildItemWithInheritedNullablesHasNullValues_NoIndexesAreCreated()
        {
            Builder = StructureBuilder.Create(c => c.Register<ChildWithNullables>());
            var item = ChildWithNullables.CreateWithNullValues();

            var structure = Builder.CreateStructure(item);

            structure.Indexes.Should().BeEmpty();
        }

        private abstract class RootWithNullables
        {
            public int? NullableInt { get; set; }
            public Guid? NullableGuid { get; set; }
        }

        private class ChildWithNullables : RootWithNullables
        {
            public static ChildWithNullables CreateWithNullValues()
            {
                return new ChildWithNullables();
            }

            public static ChildWithNullables CreatePopulated()
            {
                return new ChildWithNullables
                {
                    NullableInt = 42,
                    NullableGuid = Guid.Parse("e327e168-c6f5-415a-9a7b-fa82dc73c5d9")
                };
            }
        }

        private class TestItemWithNullables
        {
            public int? NullableInt { get; set; }
            public Guid? NullableGuid { get; set; }

            public static TestItemWithNullables CreateWithNullValues()
            {
                return new TestItemWithNullables();
            }

            public static TestItemWithNullables CreatePopulated()
            {
                return new TestItemWithNullables
                {
                    NullableInt = 42,
                    NullableGuid = Guid.Parse("e327e168-c6f5-415a-9a7b-fa82dc73c5d9")
                };
            }
        }
    }
}