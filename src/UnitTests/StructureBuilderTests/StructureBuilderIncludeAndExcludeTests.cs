using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Structurizer;

namespace UnitTests.StructureBuilderTests
{
    [TestClass]
    public class StructureBuilderIncludeAndExcludeTests : StructureBuilderBaseTests
    {
        [TestMethod]
        public void Should_be_able_to_index_nested_child_by_registrating_child_only()
        {
            Builder = StructureBuilder.Create(c => c.Register<MyRoot>(cfg => cfg
                .UsingIndexMode(IndexMode.Inclusive)
                .Members(e => e.OneChild.GrandChild.SomeInt)));

            var item = new MyRoot
            {
                SomeString = "Foo Bar",
                SomeInt = 1,
                OneChild = new MyChild
                {
                    SomeString = "One child",
                    SomeInt = 2,
                    GrandChild = new MyGrandChild
                    {
                        SomeString = "Grand child 2.1",
                        SomeInt = 21
                    }
                },
                ManyChildren = new List<MyChild>
                {
                    new MyChild
                    {
                        SomeString = "List Child1",
                        SomeInt = 3,
                        GrandChild = new MyGrandChild
                        {
                            SomeString = "Grand child 3.1",
                            SomeInt = 31
                        }
                    },
                    new MyChild
                    {
                        SomeString = "List Child2",
                        SomeInt = 4,
                        GrandChild = new MyGrandChild
                        {
                            SomeString = "Grand child 4.1",
                            SomeInt = 41
                        }
                    }
                }
            };

            var structure = Builder.CreateStructure(item);

            Assert.AreEqual(1, structure.Indexes.Count);
        }

        private class MyRoot
        {
            public string SomeString { get; set; }
            public int SomeInt { get; set; }
            public MyChild OneChild { get; set; }
            public List<MyChild> ManyChildren { get; set; }
        }

        private class MyChild
        {
            public MyGrandChild GrandChild { get; set; }
            public string SomeString { get; set; }
            public int SomeInt { get; set; }
        }

        private class MyGrandChild
        {
            public string SomeString { get; set; }
            public int SomeInt { get; set; }
        }
    }
}