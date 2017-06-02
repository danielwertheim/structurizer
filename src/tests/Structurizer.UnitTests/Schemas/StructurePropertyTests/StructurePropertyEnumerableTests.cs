using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Structurizer.UnitTests.Schemas.StructurePropertyTests
{
    [TestClass]
    public class StructurePropertyEnumerableTests : UnitTests
    {
        [TestMethod]
        public void IsEnumerable_WhenIEnumerableOfSimpleType_ReturnsTrue()
        {
            var property = GetProperty<DummyForEnumerableTests>("Ints");

            Assert.IsTrue(property.IsEnumerable);
        }

        [TestMethod]
        public void IsEnumerable_WhenPrimitiveType_ReturnsFalse()
        {
            var property = GetProperty<DummyForEnumerableTests>("Int1");

            Assert.IsFalse(property.IsEnumerable);
        }

        private static IStructureProperty GetProperty<T>(string name) where T : class
        {
            return StructurePropertyTestFactory.GetPropertyByPath<T>(name);
        }

        private class DummyForEnumerableTests
        {
            public int Int1 { get; set; }

            public IEnumerable<int> Ints { get; set; }
        }
    }
}