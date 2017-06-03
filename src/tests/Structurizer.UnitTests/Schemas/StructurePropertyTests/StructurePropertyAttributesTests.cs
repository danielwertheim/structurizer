using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Structurizer.UnitTests.Schemas.StructurePropertyTests
{
    [TestClass]
    public class StructurePropertyAttributesTests : UnitTests
    {
        [TestMethod]
        public void Should_have_two_attributes_When_property_has_two_attributes()
        {
            var property = GetProperty<DummyForAttributesTests>("SomePropWithAttributes");

            property.Attributes.Should().Contain(new My1Attribute());
            property.Attributes.Should().Contain(new System.ComponentModel.DescriptionAttribute("test"));
        }

        [TestMethod]
        public void Should_have_no_attributes_When_property_has_no_properties()
        {
            var property = GetProperty<DummyForAttributesTests>("SomePropWithoutAttributes");

            property.Attributes.Should().BeEmpty();
        }

        private static IStructureProperty GetProperty<T>(string name) where T : class
        {
            return StructurePropertyTestFactory.GetPropertyByPath<T>(name);
        }

        private class DummyForAttributesTests
        {
            [My1, System.ComponentModel.Description("test")]
            public int SomePropWithAttributes { get; set; }

            public int SomePropWithoutAttributes { get; set; }
        }

        [AttributeUsage(AttributeTargets.Property)]
        public class My1Attribute : Attribute { }
    }
}