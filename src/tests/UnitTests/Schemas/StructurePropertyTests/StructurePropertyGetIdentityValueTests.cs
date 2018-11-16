using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Schemas.StructurePropertyTests
{
    [TestClass]
    public class StructurePropertyGetIdentityValueTests : UnitTests
    {
        [TestMethod]
        public void GetIdValue_WhenIntOnFirstLevel_ReturnsInt()
        {
            const int expected = 42;
            var item = new IdentityOnRoot { Value = expected };
            var property = StructurePropertyTestFactory.GetPropertyByPath<IdentityOnRoot>("Value");
            
            var actual = property.GetValue(item);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetIdValue_WhenNullableIntOnFirstLevel_ReturnsInt()
        {
            const int expectedInt = 42;
            var item = new NullableIdentityOnRoot { Value = expectedInt };
            var property = StructurePropertyTestFactory.GetPropertyByPath<NullableIdentityOnRoot>("Value");

            var actual = property.GetValue(item);

            Assert.AreEqual(expectedInt, actual);
        }

        [TestMethod]
        public void GetIdValue_WhenNullAssignedNullableIntOnFirstLevel_ReturnsInt()
        {
            var item = new NullableIdentityOnRoot { Value = null };
            var property = StructurePropertyTestFactory.GetPropertyByPath<NullableIdentityOnRoot>("Value");
            
            var actual = property.GetValue(item);

            Assert.IsNull(actual);
        }

        private class IdentityOnRoot
        {
            public int Value { get; set; }
        }

        private class NullableIdentityOnRoot
        {
            public int? Value { get; set; }
        }
    }
}