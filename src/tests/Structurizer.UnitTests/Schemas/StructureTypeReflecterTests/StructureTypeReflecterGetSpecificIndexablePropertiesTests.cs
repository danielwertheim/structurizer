using System;
using System.Linq;
using NUnit.Framework;

namespace Structurizer.UnitTests.Schemas.StructureTypeReflecterTests
{
    [TestFixture]
    public class StructureTypeReflecterGetSpecificIndexablePropertiesTests : StructureTypeReflecterTestsBase
    {
        [Test]
        public void GetSpecificIndexableProperties_WhenCalledWithNullExlcudes_ThrowsArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() => ReflecterFor().GetSpecificIndexableProperties(typeof(TestItem), null));

            Assert.AreEqual("memberPaths", ex.ParamName);
        }

        [Test]
        public void GetSpecificIndexableProperties_WhenCalledWithNoExlcudes_ThrowsArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() => ReflecterFor().GetSpecificIndexableProperties(typeof(TestItem), new string[] { }));

            Assert.AreEqual("memberPaths", ex.ParamName);
        }

        [Test]
        public void GetSpecificIndexableProperties_WhenIncludingBytesArray_PropertyIsNotReturned()
        {
            var properties = ReflecterFor().GetSpecificIndexableProperties(typeof(TestItem), new[] { "Bytes1" });

            Assert.AreEqual(0, properties.Length);
            Assert.IsNull(properties.SingleOrDefault(p => p.Path == "Bytes1"));
        }

        [Test]
        public void GetSpecificIndexableProperties_WhenIncludingProperty_PropertyIsReturned()
        {
            var properties = ReflecterFor().GetSpecificIndexableProperties(typeof(TestItem), new[] { "Bool1" });

            Assert.AreEqual(1, properties.Length);
            Assert.IsNotNull(properties.SingleOrDefault(p => p.Path == "Bool1"));
        }

        [Test]
        public void GetSpecificIndexableProperties_WhenIncludingNestedProperty_PropertyIsReturned()
        {
            var properties = ReflecterFor().GetSpecificIndexableProperties(typeof(TestItem), new[] { "Nested", "Nested.Int1OnNested" });

            Assert.AreEqual(1, properties.Length);
            Assert.IsNotNull(properties.SingleOrDefault(p => p.Path == "Nested.Int1OnNested"));
        }

        private class TestItem
        {
            public int Int1 { get; set; }
            public bool Bool1 { get; set; }
            public DateTime DateTime1 { get; set; }
            public string String1 { get; set; }
            public byte[] Bytes1 { get; set; }
            public Nested Nested { get; set; }
        }

        private class Nested
        {
            public int Int1OnNested { get; set; }
        }
    }
}