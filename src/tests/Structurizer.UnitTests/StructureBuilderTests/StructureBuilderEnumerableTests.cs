using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Structurizer.UnitTests.StructureBuilderTests
{
    [TestClass]
    public class StructureBuilderEnumerableTests : StructureBuilderBaseTests
    {
        [TestMethod]
        public void CreateStructure_WhenEnumerableIntsOnFirstLevel_ReturnsOneIndexPerElementInCorrectOrder()
        {
            Builder = StructureBuilder.Create(c => c.Register<TestItemForFirstLevel>());
            var item = new TestItemForFirstLevel { IntArray = new[] { 5, 6, 7 } };

            var structure = Builder.CreateStructure(item);

            var indices = structure.Indexes.Where(i => i.Path.StartsWith("IntArray[")).ToList();
            Assert.AreEqual(5, indices[0].Value);
            Assert.AreEqual(6, indices[1].Value);
            Assert.AreEqual(7, indices[2].Value);
        }

        [TestMethod]
        public void CreateStructure_WhenEnumerableIntsOnFirstLevelAreNull_ReturnsNoIndex()
        {
            Builder = StructureBuilder.Create(c => c.Register<TestItemForFirstLevel>());
            var item = new TestItemForFirstLevel { IntArray = null };

            var structure = Builder.CreateStructure(item);

            var actual = structure.Indexes.SingleOrDefault(si => si.Path.StartsWith("IntArray"));
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void CreateStructure_WhenEnumerableIntsOnSecondLevelAreNull_ReturnsNoIndex()
        {
            Builder = StructureBuilder.Create(c => c.Register<TestItemForSecondLevel>());
            var item = new TestItemForSecondLevel { Container = new Container { IntArray = null } };

            var structure = Builder.CreateStructure(item);

            var actual = structure.Indexes.SingleOrDefault(si => si.Path.StartsWith("Container.IntArray"));
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void CreateStructure_WhenEnumerableIntsOnSecondLevel_ReturnsOneIndexPerElementInCorrectOrder()
        {
            Builder = StructureBuilder.Create(c => c.Register<TestItemForSecondLevel>());
            var item = new TestItemForSecondLevel { Container = new Container { IntArray = new[] { 5, 6, 7 } } };

            var structure = Builder.CreateStructure(item);

            var indices = structure.Indexes.Where(i => i.Path.StartsWith("Container.IntArray[")).ToList();
            Assert.AreEqual(5, indices[0].Value);
            Assert.AreEqual(6, indices[1].Value);
            Assert.AreEqual(7, indices[2].Value);
        }

        [TestMethod]
        public void CreateStructure_WhenCustomNonGenericEnumerableWithComplexElement_ReturnsIndexesForElements()
        {
            Builder = StructureBuilder.Create(c => c.Register<ModelForMyCustomNonGenericEnumerable>());
            var item = new ModelForMyCustomNonGenericEnumerable();
            item.Items.Add(new MyElement { IntValue = 1, StringValue = "A" });
            item.Items.Add(new MyElement { IntValue = 2, StringValue = "B" });

            var structure = Builder.CreateStructure(item);

            var indices = structure.Indexes;
            Assert.AreEqual("A", indices[0].Value);
            Assert.AreEqual("B", indices[1].Value);
            Assert.AreEqual(1, indices[2].Value);
            Assert.AreEqual(2, indices[3].Value);
        }

        [TestMethod]
        public void CreateStructure_WhenHashSetOfInts_ReturnsOneIndexPerElementInCorrectOrder()
        {
            Builder = StructureBuilder.Create(c => c.Register<TestItemWithHashSet>());
            var item = new TestItemWithHashSet { HashSetOfInts = new HashSet<int> { 5, 6, 7 } };

            var structure = Builder.CreateStructure(item);

            var indices = structure.Indexes.Where(i => i.Path.StartsWith("HashSetOfInts[")).ToList();
            Assert.AreEqual(5, indices[0].Value);
            Assert.AreEqual(6, indices[1].Value);
            Assert.AreEqual(7, indices[2].Value);
        }

        [TestMethod]
        public void CreateStructure_WhenHashSetOfIntsIsNull_ReturnsNoIndex()
        {
            Builder = StructureBuilder.Create(c => c.Register<TestItemWithHashSet>());
            var item = new TestItemWithHashSet { HashSetOfInts = null };

            var structure = Builder.CreateStructure(item);

            var actual = structure.Indexes.SingleOrDefault(si => si.Path.StartsWith("HashSetOfInts"));
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void CreateStructure_WhenISetOfInts_ReturnsOneIndexPerElementInCorrectOrder()
        {
            Builder = StructureBuilder.Create(c => c.Register<TestItemWithISet>());
            var item = new TestItemWithISet { SetOfInts = new HashSet<int> { 5, 6, 7 } };

            var structure = Builder.CreateStructure(item);

            var indices = structure.Indexes.Where(i => i.Path.StartsWith("SetOfInts[")).ToList();
            Assert.AreEqual(5, indices[0].Value);
            Assert.AreEqual(6, indices[1].Value);
            Assert.AreEqual(7, indices[2].Value);
        }

        [TestMethod]
        public void CreateStructure_WhenSetOfIntsIsNull_ReturnsNoIndex()
        {
            Builder = StructureBuilder.Create(c => c.Register<TestItemWithISet>());
            var item = new TestItemWithISet { SetOfInts = null };

            var structure = Builder.CreateStructure(item);

            var actual = structure.Indexes.SingleOrDefault(si => si.Path.StartsWith("SetOfInts"));
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void CreateStructure_WhenHashSetOfComplex_ReturnsOneIndexPerElementInCorrectOrder()
        {
            Builder = StructureBuilder.Create(c => c.Register<TestItemWithHashSetOfComplex>());
            var item = new TestItemWithHashSetOfComplex
            {
                HashSetOfComplex = new HashSet<Value>
                {
                    new Value { Is = 5 },
                    new Value { Is = 6 },
                    new Value { Is = 7 }
                }
            };

            var structure = Builder.CreateStructure(item);

            var indices = structure.Indexes.Where(i => i.Path.StartsWith("HashSetOfComplex[")).ToList();
            Assert.AreEqual(5, indices[0].Value);
            Assert.AreEqual(6, indices[1].Value);
            Assert.AreEqual(7, indices[2].Value);
        }

        [TestMethod]
        public void CreateStructure_WhenHashSetOfComplex_HasThreeNullItems_ReturnsNoIndex()
        {
            Builder = StructureBuilder.Create(c => c.Register<TestItemWithHashSetOfComplex>());
            var item = new TestItemWithHashSetOfComplex { HashSetOfComplex = new HashSet<Value> { null, null, null } };

            var structure = Builder.CreateStructure(item);

            var actual = structure.Indexes.SingleOrDefault(si => si.Path.StartsWith("HashSetOfComplex.Is"));
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void CreateStructure_WhenIDictionary_ReturnsOneIndexPerElementInCorrectOrder()
        {
            Builder = StructureBuilder.Create(c => c.Register<TestItemWithIDictionary>());
            var item = new TestItemWithIDictionary
            {
                KeyValues = new Dictionary<string, int>
                {
                    { "Key1", 5 },
                    { "Key2", 6 },
                    { "Key3", 7 }
                }
            };

            var structure = Builder.CreateStructure(item);

            var indices = structure.Indexes.Where(i => i.Path.StartsWith("KeyValues")).ToList();
            Assert.AreEqual(6, indices.Count);

            Assert.AreEqual("KeyValues[0].Key", indices[0].Path);
            Assert.AreEqual("Key1", indices[0].Value);
            Assert.AreEqual("KeyValues[1].Key", indices[1].Path);
            Assert.AreEqual("Key2", indices[1].Value);
            Assert.AreEqual("KeyValues[2].Key", indices[2].Path);
            Assert.AreEqual("Key3", indices[2].Value);

            Assert.AreEqual("KeyValues[0].Value", indices[3].Path);
            Assert.AreEqual(5, indices[3].Value);
            Assert.AreEqual("KeyValues[1].Value", indices[4].Path);
            Assert.AreEqual(6, indices[4].Value);
            Assert.AreEqual("KeyValues[2].Value", indices[5].Path);
            Assert.AreEqual(7, indices[5].Value);
        }

        [TestMethod]
        public void CreateStructure_WhenDictionary_ReturnsOneIndexPerElementInCorrectOrder()
        {
            Builder = StructureBuilder.Create(c => c.Register<TestItemWithDictionary>());
            var item = new TestItemWithDictionary { KeyValues = new Dictionary<string, int> { { "Key1", 5 }, { "Key2", 6 }, { "Key3", 7 } } };

            var structure = Builder.CreateStructure(item);

            var indices = structure.Indexes.Where(i => i.Path.StartsWith("KeyValues")).ToList();
            Assert.AreEqual(6, indices.Count);

            Assert.AreEqual("KeyValues[0].Key", indices[0].Path);
            Assert.AreEqual("Key1", indices[0].Value);
            Assert.AreEqual("KeyValues[1].Key", indices[1].Path);
            Assert.AreEqual("Key2", indices[1].Value);
            Assert.AreEqual("KeyValues[2].Key", indices[2].Path);
            Assert.AreEqual("Key3", indices[2].Value);

            Assert.AreEqual("KeyValues[0].Value", indices[3].Path);
            Assert.AreEqual(5, indices[3].Value);
            Assert.AreEqual("KeyValues[1].Value", indices[4].Path);
            Assert.AreEqual(6, indices[4].Value);
            Assert.AreEqual("KeyValues[2].Value", indices[5].Path);
            Assert.AreEqual(7, indices[5].Value);
        }

        [TestMethod]
        public void CreateStructure_WhenIDictionaryWithComplex_ReturnsOneIndexPerElementInCorrectOrder()
        {
            Builder = StructureBuilder.Create(c => c.Register<TestItemWithIDictionaryOfComplex>());
            var item = new TestItemWithIDictionaryOfComplex
            {
                KeyValues = new Dictionary<string, Value>
                {
                    { "Key1", new Value { Is = 5 } },
                    { "Key2", new Value { Is = 6 } },
                    { "Key3", new Value { Is = 7 } }
                }
            };

            var structure = Builder.CreateStructure(item);

            var indices = structure.Indexes.Where(i => i.Path.StartsWith("KeyValues")).ToList();
            Assert.AreEqual(6, indices.Count);

            Assert.AreEqual("KeyValues[0].Key", indices[0].Path);
            Assert.AreEqual("Key1", indices[0].Value);
            Assert.AreEqual("KeyValues[1].Key", indices[1].Path);
            Assert.AreEqual("Key2", indices[1].Value);
            Assert.AreEqual("KeyValues[2].Key", indices[2].Path);
            Assert.AreEqual("Key3", indices[2].Value);

            Assert.AreEqual("KeyValues[0].Value.Is", indices[3].Path);
            Assert.AreEqual(5, indices[3].Value);
            Assert.AreEqual("KeyValues[1].Value.Is", indices[4].Path);
            Assert.AreEqual(6, indices[4].Value);
            Assert.AreEqual("KeyValues[2].Value.Is", indices[5].Path);
            Assert.AreEqual(7, indices[5].Value);
        }

        [TestMethod]
        public void CreateStructure_WhenDictionaryWithComplex_ReturnsOneIndexPerElementInCorrectOrder()
        {
            Builder = StructureBuilder.Create(c => c.Register<TestItemWithDictionaryOfComplex>());
            var item = new TestItemWithDictionaryOfComplex { KeyValues = new Dictionary<string, Value> { { "Key1", new Value { Is = 5 } }, { "Key2", new Value { Is = 6 } }, { "Key3", new Value { Is = 7 } } } };

            var structure = Builder.CreateStructure(item);

            var indexes = structure.Indexes.Where(i => i.Path.StartsWith("KeyValues")).ToList();
            Assert.AreEqual(6, indexes.Count);

            Assert.AreEqual("KeyValues[0].Key", indexes[0].Path);
            Assert.AreEqual("Key1", indexes[0].Value);
            Assert.AreEqual("KeyValues[1].Key", indexes[1].Path);
            Assert.AreEqual("Key2", indexes[1].Value);
            Assert.AreEqual("KeyValues[2].Key", indexes[2].Path);
            Assert.AreEqual("Key3", indexes[2].Value);

            Assert.AreEqual("KeyValues[0].Value.Is", indexes[3].Path);
            Assert.AreEqual(5, indexes[3].Value);
            Assert.AreEqual("KeyValues[1].Value.Is", indexes[4].Path);
            Assert.AreEqual(6, indexes[4].Value);
            Assert.AreEqual("KeyValues[2].Value.Is", indexes[5].Path);
            Assert.AreEqual(7, indexes[5].Value);
        }

        private class ModelForMyCustomNonGenericEnumerable
        {
            public MyCustomNonGenericEnumerable Items { get; set; }

            public ModelForMyCustomNonGenericEnumerable()
            {
                Items = new MyCustomNonGenericEnumerable();
            }
        }

        private class MyCustomNonGenericEnumerable : System.Collections.Generic.List<MyElement>
        {
        }

        private class MyElement
        {
            public string StringValue { get; set; }
            public int IntValue { get; set; }
        }

        private class TestItemForFirstLevel
        {
            public int IntValue { get; set; }

            public int[] IntArray { get; set; }
        }

        private class TestItemForSecondLevel
        {
            public Container Container { get; set; }
        }

        private class Container
        {
            public int IntValue { get; set; }

            public int[] IntArray { get; set; }
        }

        private class Value
        {
            public int Is { get; set; }
        }

        private class TestItemWithHashSetOfComplex
        {
            public HashSet<Value> HashSetOfComplex { get; set; }
        }

        private class TestItemWithHashSet
        {
            public HashSet<int> HashSetOfInts { get; set; }
        }

        private class TestItemWithISet
        {
            public ISet<int> SetOfInts { get; set; }
        }

        private class TestItemWithIDictionary
        {
            public IDictionary<string, int> KeyValues { get; set; }
        }

        private class TestItemWithIDictionaryOfComplex
        {
            public IDictionary<string, Value> KeyValues { get; set; }
        }

        private class TestItemWithDictionary
        {
            public Dictionary<string, int> KeyValues { get; set; }
        }

        private class TestItemWithDictionaryOfComplex
        {
            public Dictionary<string, Value> KeyValues { get; set; }
        }

        private class TestItemWithIntAsId
        {
            public int IntValue { get; set; }
        }

        private class TestItemWithLongAsId
        {
            public int IntValue { get; set; }
        }
    }
}