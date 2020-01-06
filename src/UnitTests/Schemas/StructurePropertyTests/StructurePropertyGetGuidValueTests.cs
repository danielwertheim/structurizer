using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Schemas.StructurePropertyTests
{
    [TestClass]
    public class StructurePropertyGetGuidValueTests : UnitTests
    {
        [TestMethod]
        public void GetIdValue_WhenGuidOnFirstLevel_ReturnsGuid()
        {
            var expected = Guid.Parse("4217F3B7-6DEB-4DFA-B195-D111C1297988");
            var item = new GuidOnRoot { Value = expected };
            var property = StructurePropertyTestFactory.GetPropertyByPath<GuidOnRoot>("Value");
            
            var actual = property.GetValue(item);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetIdValue_WhenNullableGuidOnFirstLevel_ReturnsGuid()
        {
            var expected = Guid.Parse("4217F3B7-6DEB-4DFA-B195-D111C1297988");
            var item = new NullableGuidOnRoot { Value = expected };
            var property = StructurePropertyTestFactory.GetPropertyByPath<NullableGuidOnRoot>("Value");
            
            var actual = property.GetValue(item);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetIdValue_WhenNullAssignedNullableGuidOnFirstLevel_ReturnsNull()
        {
            var item = new NullableGuidOnRoot { Value = null };
            var property = StructurePropertyTestFactory.GetPropertyByPath<NullableGuidOnRoot>("Value");
            
            var actual = property.GetValue(item);

            Assert.IsNull(actual);
        }

        private class GuidOnRoot
        {
            public Guid Value { get; set; }
        }

        private class NullableGuidOnRoot
        {
            public Guid? Value { get; set; }
        }

        private class Container
        {
            public GuidOnRoot GuidOnRootItem { get; set; }
        }
    }
}