using System;
using System.Collections.Generic;
using Structurizer;
using Structurizer.Configuration;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var order = new Order
            {
                Id = 1,
                OrderSimpleInts = new[] { 1, 2, 3 },
                OrderNo = "2016-1234",
                PlacedAt = DateTime.Now,
                Lines = new List<OrderLine>
                {
                    new OrderLine
                    {
                        OrderLineSimpleInts = new [] {11,12,13},
                        ArticleNo = "Article-Line0",
                        Qty = 42,
                        Items =  new List<Item>
                        {
                            new Item
                            {
                                Name = "Key-Line0-Item0",
                                Value = "Value-Line0-Item0",
                                ItemSimpleInts = new [] {101, 102, 103}
                            },
                            new Item
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
                        Items =  new List<Item>
                        {
                            new Item
                            {
                                Name = "Key-Line1-Item0",
                                Value = "Value-Line1-Item0"
                            },
                            new Item
                            {
                                Name = "Key-Line1-Item1",
                                Value = "Value-Line1-Item1"
                            }
                        }
                    }
                }
            };

            var typeConfigs = new StructureTypeConfigurations();
            typeConfigs.Register<Order>();
            typeConfigs.Register<Foo>();

            //typeConfigs.Register<Order>(cfg => cfg
            //    .UsingIndexMode(IndexMode.Inclusive)
            //    .Members(i => i.OrderNo));
            //.Members(i => i.Lines[0].Items[0].Name));

            //inclusive:Lines.*
            //inclusive:Lines.ArticleNo
            //exclusive:Lines.*
            //exclusive.Lines.ArticleNo

            var structureBuilder = StructureBuilder.Create(typeConfigs);
            var orderStructure = structureBuilder.CreateStructure(order);
            var fooStructure = structureBuilder.CreateStructure(new Foo
            {
                FooScore = 1,
                Bar = new Bar
                {
                    BarScore = new[] { 2, 3 }
                }
            });

            Console.WriteLine($"===== {orderStructure.Name} =====");
            foreach (var index in orderStructure.Indexes)
                Console.WriteLine(DefaultIndexValueFormatter.Format(index));

            Console.WriteLine($"===== {fooStructure.Name} =====");
            foreach (var index in fooStructure.Indexes)
                Console.WriteLine(DefaultIndexValueFormatter.Format(index));

            Console.ReadLine();
        }
    }

    public static class DefaultIndexValueFormatter
    {
        public static string Format(IStructureIndex index)
        {
            var path = index.Path;

            switch (index.DataTypeCode)
            {
                case DataTypeCode.String:
                case DataTypeCode.Guid:
                case DataTypeCode.Enum:
                    return $"{path}=\"{index.Value}\"";
                case DataTypeCode.DateTime:
                    return $"{path}=\"{((DateTime)index.Value):O}\"";
                default:
                    return $"{path}={index.Value}";
            }
        }
    }

    public interface IOrder
    {
        int Id { get; set; }
        string OrderNo { get; set; }
        DateTime PlacedAt { get; set; }
        List<OrderLine> Lines { get; set; }
        int[] OrderSimpleInts { get; set; }
    }

    public class Order : IOrder
    {
        public int Id { get; set; }
        public string OrderNo { get; set; }
        public DateTime PlacedAt { get; set; }
        public List<OrderLine> Lines { get; set; }
        public int[] OrderSimpleInts { get; set; }
    }

    public class OrderLine
    {
        public string ArticleNo { get; set; }
        public int Qty { get; set; }
        public List<Item> Items { get; set; }
        public int[] OrderLineSimpleInts { get; set; }
    }

    public class Item
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int[] ItemSimpleInts { get; set; }
    }

    public class Foo
    {
        public int FooScore { get; set; }
        public Bar Bar { get; set; }
    }

    public class Bar
    {
        public int[] BarScore { get; set; }
    }
}
