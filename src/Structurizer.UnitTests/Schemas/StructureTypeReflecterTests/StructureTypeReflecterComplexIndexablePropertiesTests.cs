using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Structurizer.UnitTests.Schemas.StructureTypeReflecterTests
{
    [TestFixture]
    public class StructureTypeReflecterComplexIndexablePropertiesTests : StructureTypeReflecterTestsBase
    {
        [Test]
        public void GetIndexableProperties_WhenItemWithComplexProperty_ReturnsComplexProperties()
        {
            var properties = ReflecterFor().GetIndexableProperties(typeof(WithComplexProperty));

            Assert.AreEqual(2, properties.Count());
            Assert.IsTrue(properties.Any(p => p.Path == "Complex.IntValue"));
            Assert.IsTrue(properties.Any(p => p.Path == "Complex.StringValue"));
        }

        [Test]
        public void GetIndexableProperties_WhenRootWithEnumerable_EnumerableMemberIsNotReturnedAsComplex()
        {
            var properties = ReflecterFor().GetIndexableProperties(typeof(WithEnumerableOfComplex));

            Assert.AreEqual(2, properties.Count());
            Assert.IsTrue(properties.Any(p => p.Path == "Items.IntValue"));
            Assert.IsTrue(properties.Any(p => p.Path == "Items.StringValue"));
        }

        private class WithComplexProperty
        {
            public Item Complex { get; set; }
        }

        private class WithEnumerableOfComplex
        {
            public IEnumerable<Item> Items { get; set; }
        }

        private class Item
        {
            public int IntValue { get; set; }
            public string StringValue { get; set; }
        }
    }
}