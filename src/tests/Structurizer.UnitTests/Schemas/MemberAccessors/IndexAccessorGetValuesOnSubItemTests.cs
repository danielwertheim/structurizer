using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Structurizer.UnitTests.Schemas.MemberAccessors
{
    [TestFixture]
    public class IndexAccessorGetValuesOnSubItemTests : UnitTestBase
    {
        [Test]
        public void GetValues_WhenSubItemsArrayHasElementsWithValues_ReturnsTheValues()
        {
            var subItems = new[] { new SubItem { Value = "A" }, new SubItem { Value = "B" } };
            var item = new Item { SubItems = subItems };
            var valueProp = StructurePropertyTestFactory.GetPropertyByPath<Item>("SubItems.Value");
            var indexAccessor = IndexAccessorTestFactory.CreateFor(valueProp);

            var values = indexAccessor.GetValues(item);

            CollectionAssert.AreEquivalent(new[] { "A", "B" }, values.Select(i => i.Value));
        }

        [Test]
        public void GetValues_WhenSubItemsArrayHasElementsWithNullValues_DoesNotReturnTheNullValues()
        {
            var subItems = new[] { new SubItem { Value = null }, new SubItem { Value = null } };
            var item = new Item { SubItems = subItems };
            var valueProp = StructurePropertyTestFactory.GetPropertyByPath<Item>("SubItems.Value");
            var indexAccessor = IndexAccessorTestFactory.CreateFor(valueProp);

            var values = indexAccessor.GetValues(item);

            values.Should().BeEmpty();
        }

        [Test]
        public void GetValues_WhenSubItemsArrayIsNull_ReturnsNull()
        {
            var item = new Item { SubItems = null };
            var valueProp = StructurePropertyTestFactory.GetPropertyByPath<Item>("SubItems.Value");
            var indexAccessor = IndexAccessorTestFactory.CreateFor(valueProp);

            var value = indexAccessor.GetValues(item);

            value.Should().BeNull();
        }

        [Test]
        public void GetValues_WhenSubItemsArrayHasBothNullAndNonNullItems_ReturnsNonNullItemsOnly()
        {
            var subItems = new[] { null, new SubItem { Value = "A" } };
            var item = new Item { SubItems = subItems };
            var valueProp = StructurePropertyTestFactory.GetPropertyByPath<Item>("SubItems.Value");
            var indexAccessor = IndexAccessorTestFactory.CreateFor(valueProp);

            var value = indexAccessor.GetValues(item);

            Assert.AreEqual(new object[] { "A" }, value.Select(i => i.Value));
        }

        [Test]
        public void GetValues_WhenStringOnSingleSubItem_ReturnsValue()
        {
            var subItem = new SubItem { Value = "The value" };
            var item = new Item { SingleSubItem = subItem };
            var valueProp = StructurePropertyTestFactory.GetPropertyByPath<Item>("SingleSubItem.Value");
            var indexAccessor = IndexAccessorTestFactory.CreateFor(valueProp);

            var value = indexAccessor.GetValues(item);

            Assert.AreEqual(new object[] { "The value" }, value.Select(i => i.Value));
        }

        private class Item
        {
            public SubItem SingleSubItem { get; set; }

            public SubItem[] SubItems { get; set; }
        }

        private class SubItem
        {
            public string Value { get; set; }
        }
    }
}