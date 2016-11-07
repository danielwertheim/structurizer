using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Structurizer.UnitTests.StructureBuilderTests
{
    [TestFixture]
    public class StructureBuilderTests : StructureBuilderBaseTests
    {
        [Test]
        public void CreateStructure_WhenIntOnFirstLevel_ReturnsSimpleValue()
        {
            Builder = StructureBuilder.Create(c => c.Register<TestItemForFirstLevel>());
            var item = new TestItemForFirstLevel { IntValue = 42 };

            var structure = Builder.CreateStructure(item);

            var actual = structure.Indexes.Single(si => si.Path == "IntValue").Node;
            Assert.AreEqual(42, actual);
        }

        [Test]
        public void CreateStructure_WhenUIntOnFirstLevel_ReturnsSimpleValue()
        {
            Builder = StructureBuilder.Create(c => c.Register<TestItemForFirstLevel>());
            var item = new TestItemForFirstLevel { UIntValue = 42 };

            var structure = Builder.CreateStructure(item);

            var actual = structure.Indexes.Single(si => si.Path == "UIntValue").Node;
            Assert.AreEqual(42, actual);
        }

        [Test]
        public void CreateStructure_WhenIntOnSecondLevel_ReturnsSimpleValue()
        {
            Builder = StructureBuilder.Create(c => c.Register<TestItemForSecondLevel>());
            var item = new TestItemForSecondLevel { Container = new Container { IntValue = 42 } };

            var structure = Builder.CreateStructure(item);

            var actual = structure.Indexes.Single(si => si.Path == "Container.IntValue").Node;
            Assert.AreEqual(42, actual);
        }

        [Test]
        public void CreateStructure_WhenStructureContainsStructWithValue_ValueOfStructIsRepresentedInIndex()
        {
            Builder = StructureBuilder.Create(c => c.Register<IHaveStruct>());
            var item = new IHaveStruct { Content = "My content" };

            var structure = Builder.CreateStructure(item);

            Assert.AreEqual(1, structure.Indexes.Count);
            Assert.AreEqual("Content", structure.Indexes[0].Path);
            Assert.AreEqual(typeof(MyText), structure.Indexes[0].DataType);
            Assert.AreEqual(new MyText("My content"), structure.Indexes[0].Node);
        }

        [Test]
        public void CreateStructure_When_structure_has_null_collection_It_should_create_structure_with_index_for_other_members()
        {
            Builder = StructureBuilder.Create(c => c.Register<WithNullCollection>());
            var item = new WithNullCollection { Temp = "Foo", Values = null };

            var structure = Builder.CreateStructure(item);

            Assert.AreEqual(1, structure.Indexes.Count);
            Assert.AreEqual("Temp", structure.Indexes[0].Path);
        }

        private class TestItemForFirstLevel
        {
            public int IntValue { get; set; }
            public uint UIntValue { get; set; }
        }

        private class TestItemForSecondLevel
        {
            public Container Container { get; set; }
        }

        private class Container
        {
            public int IntValue { get; set; }
        }

        private class IHaveStruct
        {
            public MyText Content { get; set; }
        }

        public class WithNullCollection
        {
            public List<Item> Values { get; set; }
            public string Temp { get; set; }
        }

        public class Item
        {
            public string Value { get; set; }
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