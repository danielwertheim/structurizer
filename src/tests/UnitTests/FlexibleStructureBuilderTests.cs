using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Structurizer;

namespace UnitTests
{
    [TestClass]
    public class FlexibleStructureBuilderTests : UnitTestsOf<FlexibleStructureBuilder>
    {
        [TestInitialize]
        public virtual void OnTestInit()
        {
            UnitUnderTest = new FlexibleStructureBuilder();
        }

        [TestMethod]
        public void Should_be_able_to_create_a_structure_When_type_is_unknown()
        {
            var item = new MyType
            {
                MyInt = 42,
                MyString = "Foo"
            };

            var structure = UnitUnderTest.CreateStructure(item);

            structure.Name.Should().Be(nameof(MyType));
            structure.Indexes.Should().HaveCount(2);
            structure.Indexes.Should()
                .Contain(i => i.Name == nameof(item.MyInt))
                .Which.Value.Should().Be(item.MyInt);
            structure.Indexes.Should()
                .Contain(i => i.Name == nameof(item.MyString))
                .Which.Value.Should().Be(item.MyString);
        }

        [TestMethod]
        public void Should_be_able_to_create_a_structure_When_type_is_anonymous()
        {
            var item = new
            {
                MyInt = 42,
                MyString = "Foo"
            };

            var structure = UnitUnderTest.CreateStructure(item);

            structure.Name.Should().Contain("Anonymous");
            structure.Indexes.Should().HaveCount(2);
            structure.Indexes.Should()
                .Contain(i => i.Name == nameof(item.MyInt))
                .Which.Value.Should().Be(item.MyInt);
            structure.Indexes.Should()
                .Contain(i => i.Name == nameof(item.MyString))
                .Which.Value.Should().Be(item.MyString);
        }

        [TestMethod]
        public void Should_be_able_to_reconfigure_using_type()
        {
            var item = new MyType
            {
                MyInt = 42,
                MyString = "Foo"
            };

            var structure = UnitUnderTest.CreateStructure(item);
            structure.Name.Should().Be(nameof(MyType));
            structure.Indexes.Should().HaveCount(2);
            structure.Indexes.Should()
                .Contain(i => i.Name == nameof(item.MyInt))
                .Which.Value.Should().Be(item.MyInt);
            structure.Indexes.Should()
                .Contain(i => i.Name == nameof(item.MyString))
                .Which.Value.Should().Be(item.MyString);

            UnitUnderTest.Configure(typeof(MyType), cfg => cfg.Members("MyInt").UsingIndexMode(IndexMode.Inclusive));

            structure = UnitUnderTest.CreateStructure(item);
            structure.Name.Should().Be(nameof(MyType));
            structure.Indexes.Should().HaveCount(1);
            structure.Indexes.Should()
                .Contain(i => i.Name == nameof(item.MyInt))
                .Which.Value.Should().Be(item.MyInt);
        }

        [TestMethod]
        public void Should_be_able_to_reconfigure_using_generics_type()
        {
            var item = new MyType
            {
                MyInt = 42,
                MyString = "Foo"
            };

            var structure = UnitUnderTest.CreateStructure(item);
            structure.Name.Should().Be(nameof(MyType));
            structure.Indexes.Should().HaveCount(2);
            structure.Indexes.Should()
                .Contain(i => i.Name == nameof(item.MyInt))
                .Which.Value.Should().Be(item.MyInt);
            structure.Indexes.Should()
                .Contain(i => i.Name == nameof(item.MyString))
                .Which.Value.Should().Be(item.MyString);

            UnitUnderTest.Configure<MyType>(cfg => cfg.Members("MyInt").UsingIndexMode(IndexMode.Inclusive));

            structure = UnitUnderTest.CreateStructure(item);
            structure.Name.Should().Be(nameof(MyType));
            structure.Indexes.Should().HaveCount(1);
            structure.Indexes.Should()
                .Contain(i => i.Name == nameof(item.MyInt))
                .Which.Value.Should().Be(item.MyInt);
        }

        [TestMethod]
        public void Should_be_able_to_reconfigure_using_anonymous_type()
        {
            var item = new
            {
                MyInt = 42,
                MyString = "Foo"
            };

            var structure = UnitUnderTest.CreateStructure(item);
            structure.Name.Should().Contain("Anonymous");
            structure.Indexes.Should().HaveCount(2);
            structure.Indexes.Should()
                .Contain(i => i.Name == nameof(item.MyInt))
                .Which.Value.Should().Be(item.MyInt);
            structure.Indexes.Should()
                .Contain(i => i.Name == nameof(item.MyString))
                .Which.Value.Should().Be(item.MyString);

            UnitUnderTest.ConfigureUsingTemplate(item, cfg => cfg.Members(i => i.MyInt).UsingIndexMode(IndexMode.Inclusive));

            structure = UnitUnderTest.CreateStructure(item);
            structure.Name.Should().Contain("Anonymous");
            structure.Indexes.Should().HaveCount(1);
            structure.Indexes.Should()
                .Contain(i => i.Name == nameof(item.MyInt))
                .Which.Value.Should().Be(item.MyInt);
        }

        private class MyType
        {
            public int MyInt { get; set; }
            public string MyString { get; set; }
        }
    }
}