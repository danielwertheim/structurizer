using NUnit.Framework;

namespace Structurizer.UnitTests.Schemas.StructurePropertyTests
{
    [TestFixture]
    public class StructurePropertyGetIdentityValueTests : UnitTestBase
    {
        [Test]
        public void GetIdValue_WhenIntOnFirstLevel_ReturnsInt()
        {
            const int expected = 42;
            var item = new IdentityOnRoot { Value = expected };
            var property = StructurePropertyTestFactory.GetPropertyByPath<IdentityOnRoot>("Value");
            
            var actual = property.GetValue(item);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetIdValue_WhenNullableIntOnFirstLevel_ReturnsInt()
        {
            const int expectedInt = 42;
            var item = new NullableIdentityOnRoot { Value = expectedInt };
            var property = StructurePropertyTestFactory.GetPropertyByPath<NullableIdentityOnRoot>("Value");

            var actual = property.GetValue(item);

            Assert.AreEqual(expectedInt, actual);
        }

        [Test]
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