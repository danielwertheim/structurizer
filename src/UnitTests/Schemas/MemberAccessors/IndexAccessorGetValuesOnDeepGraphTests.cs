using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Schemas.MemberAccessors
{
    [TestClass]
    public class IndexAccessorGetValuesOnDeepGraphTests : UnitTests
    {
        [TestMethod]
        public void GetValues_WhenDeepGraphWithEnumerables_CanExtractValues()
        {
            var prodNoProperty = StructurePropertyTestFactory.GetPropertyByPath<TestCustomer>("Orders.Lines.ProductNo");
            var pricesProperty = StructurePropertyTestFactory.GetPropertyByPath<TestCustomer>("Orders.Lines.Prices");

            var graph = new TestCustomer
            {
                Orders =
                {
                    new TestOrder
                    {
                        Lines =
                        {
                            new TestOrderLine { ProductNo = "P1", Quantity = 1, Prices = new[] { 42, 4242 }},
                            new TestOrderLine { ProductNo = "P2", Quantity = 2, Prices = new[] { 43, 4343 }}
                        }
                    }
                }
            };

            var productNos = IndexAccessorTestFactory.CreateFor(prodNoProperty).GetValues(graph);
            var prices = IndexAccessorTestFactory.CreateFor(pricesProperty).GetValues(graph);

            CollectionAssert.AreEqual(new[] { "P1", "P2" }, productNos.Select(i => i.Value).ToArray());
            CollectionAssert.AreEqual(new[] { 42, 4242, 43, 4343 }, prices.Select(i => i.Value).ToArray());
        }

        private class TestCustomer
        {
            public string CustomerNo { get; set; }

            public List<TestOrder> Orders { get; set; }

            public int[] Points { get; set; }

            public string[] Addresses { get; set; }

            public TestCustomer()
            {
                Orders = new List<TestOrder>();
            }
        }

        private class TestOrder
        {
            public List<TestOrderLine> Lines { get; set; }

            public TestOrder()
            {
                Lines = new List<TestOrderLine>();
            }
        }

        private class TestOrderLine
        {
            public string ProductNo { get; set; }

            public int Quantity { get; set; }

            public int[] Prices { get; set; }
        }
    }
}