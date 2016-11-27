using System;
using System.Collections.Generic;

namespace Sample
{
    public class Order
    {
        public long Id { get; set; }
        public Guid MerchantId { get; set; }
        public string OrderNo { get; set; }
        public Guid? CustomerId { get; set; }
        public OrderStatus Status { get; set; }
        public bool IsShipped { get; set; }
        public DateTime PlacedAt { get; set; }
        public decimal FreightCost { get; set; }
        public decimal Amount { get; set; }
        public int Discount { get; set; }
        public decimal AmountToPay { get; set; }
        public List<OrderLine> Lines { get; set; }
        public string[] Tags { get; set; }

        public static Order CreateSample()
        {
            var order = new Order
            {
                Id = DateTime.Now.Ticks,
                MerchantId = Guid.NewGuid(),
                OrderNo = "2016-1234",
                CustomerId = Guid.NewGuid(),
                Tags = new[] { "Test1", "Test2", "Gold customer" },
                PlacedAt = DateTime.Now.Subtract(TimeSpan.FromDays(2)),
                Status = OrderStatus.Payed,
                IsShipped = true,
                FreightCost = 33.50M,
                Amount = 1300M,
                Discount = 100,
                AmountToPay = 1233.50M,
                Lines = new List<OrderLine>
                {
                    new OrderLine
                    {
                        ArticleNo = "Article-Line0",
                        Qty = 42,
                        Props =  new List<Prop>
                        {
                            new Prop
                            {
                                Name = "Key-Line0-Item0",
                                Value = "Value-Line0-Item0",
                            },
                            new Prop
                            {
                                Name = "Key-Line0-Item1",
                                Value = "Value-Line0-Item1"
                            }
                        }
                    },
                    new OrderLine
                    {
                        ArticleNo = "Article-Line1",
                        Qty = 3,
                        Props =  new List<Prop>
                        {
                            new Prop
                            {
                                Name = "Key-Line1-Item0",
                                Value = "Value-Line1-Item0"
                            },
                            new Prop
                            {
                                Name = "Key-Line1-Item1",
                                Value = "Value-Line1-Item1"
                            }
                        }
                    }
                }
            };

            return order;
        }
    }
}