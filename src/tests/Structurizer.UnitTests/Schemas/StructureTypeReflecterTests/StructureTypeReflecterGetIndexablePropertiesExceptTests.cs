using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Structurizer.UnitTests.Schemas.StructureTypeReflecterTests
{
    [TestClass]
    public class StructureTypeReflecterGetIndexablePropertiesExceptTests : StructureTypeReflecterTestsBase
    {
        [TestMethod]
        public void GetIndexablePropertiesExcept_WhenCalledWithNullExlcudes_ThrowsArgumentException()
        {
            Action a = () => ReflecterFor().GetIndexablePropertiesExcept(typeof(Item), null);

            a.ShouldThrow<ArgumentException>().Where(ex => ex.ParamName == "memberPaths");
        }

        [TestMethod]
        public void GetIndexablePropertiesExcept_WhenCalledWithNoExlcudes_ThrowsArgumentNullException()
        {
            Action a = () => ReflecterFor().GetIndexablePropertiesExcept(typeof(Item), new string[] { });

            a.ShouldThrow<ArgumentException>().Where(ex => ex.ParamName == "memberPaths");
        }

        [TestMethod]
        public void GetIndexablePropertiesExcept_WhenBytesArrayExists_PropertyIsNotReturned()
        {
            var properties = ReflecterFor().GetIndexablePropertiesExcept(typeof(Item), new[] { "" });

            Assert.IsNull(properties.SingleOrDefault(p => p.Path == "Bytes1"));
        }

        [TestMethod]
        public void GetIndexablePropertiesExcept_WhenExcludingAllProperties_NoPropertiesAreReturned()
        {
            var properties = ReflecterFor().GetIndexablePropertiesExcept(typeof(Item), new[] { "Bool1", "DateTime1", "String1", "Nested", "Nested.Int1OnNested", "Nested.String1OnNested" });

            Assert.AreEqual(0, properties.Count());
        }

        [TestMethod]
        public void GetIndexablePropertiesExcept_WhenExcludingComplexNested_NoNestedPropertiesAreReturned()
        {
            var properties = ReflecterFor().GetIndexablePropertiesExcept(typeof(Item), new[] { "Nested" });

            Assert.AreEqual(0, properties.Count(p => p.Path.StartsWith("Nested")));
        }

        [TestMethod]
        public void GetIndexablePropertiesExcept_WhenExcludingNestedSimple_OtherSimpleNestedPropertiesAreReturned()
        {
            var properties = ReflecterFor().GetIndexablePropertiesExcept(typeof(Item), new[] { "Nested.String1OnNested" });

            Assert.AreEqual(1, properties.Count(p => p.Path.StartsWith("Nested")));
            Assert.AreEqual(1, properties.Count(p => p.Path == "Nested.Int1OnNested"));
        }

        private class Item
        {
            public bool Bool1 { get; set; }
            public DateTime DateTime1 { get; set; }
            public string String1 { get; set; }
            public byte[] Bytes1 { get; set; }
            public Nested Nested { get; set; }
        }

        private class Nested
        {
            public int Int1OnNested { get; set; }
            public string String1OnNested { get; set; }
        }
    }
}