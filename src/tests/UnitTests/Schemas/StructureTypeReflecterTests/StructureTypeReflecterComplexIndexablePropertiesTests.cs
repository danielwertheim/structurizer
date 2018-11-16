using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Schemas.StructureTypeReflecterTests
{
    [TestClass]
    public class StructureTypeReflecterComplexIndexablePropertiesTests : StructureTypeReflecterTestsBase
    {
        [TestMethod]
        public void GetIndexableProperties_WhenItemWithComplexProperty_ReturnsComplexProperties()
        {
            var properties = ReflecterFor().GetIndexableProperties(typeof(WithComplexProperty));

            Assert.AreEqual(2, properties.Count());
            Assert.IsTrue(properties.Any(p => p.Path == "Complex.IntValue"));
            Assert.IsTrue(properties.Any(p => p.Path == "Complex.StringValue"));
        }

        [TestMethod]
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