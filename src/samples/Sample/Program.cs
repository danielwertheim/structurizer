using System;
using Structurizer;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            //var typeConfigs = new StructureTypeConfigurations();
            //typeConfigs.Register<Order>(cfg => cfg
            //    .UsingIndexMode(IndexMode.Exclusive)
            //    .Members(i => i.Lines[0].Props));

            //inclusive:Lines.*
            //inclusive:Lines.ArticleNo
            //exclusive:Lines.*
            //exclusive.Lines.ArticleNo



            //var structureBuilder = StructureBuilder.Create(cfg => cfg.Register<Order>());
            //var order = Order.CreateSample();
            //var orderStructure = structureBuilder.CreateStructure(order);
            //DumpStructure(orderStructure);

            var structureBuilder = StructureBuilder.Create(cfg => cfg.Register<Foo>());
            var foo = Foo.CreateSample();
            var fooStructure = structureBuilder.CreateStructure(foo);
            DumpStructure(fooStructure);

            Console.ReadLine();
        }

        private static void DumpStructure(IStructure structure)
        {
            Console.WriteLine($"===== {structure.Name} =====");
            foreach (var index in structure.Indexes)
                Console.WriteLine(DefaultIndexValueFormatter.Format(index));
        }
    }

    public static class DefaultIndexValueFormatter
    {
        public static string Format(IStructureIndex index)
        {
            return $"{index.Path}=\"{index.Value}\"";
            //switch (index.DataTypeCode)
            //{
            //    case DataTypeCode.String:
            //    case DataTypeCode.Guid:
            //    case DataTypeCode.Enum:
            //        return $"Path\t{index.Path}=\"{index.Value}\"";
            //    case DataTypeCode.DateTime:
            //        return $"Path\t{index.Path}=\"{((DateTime)index.Value):O}\"";
            //    default:
            //        return $"Path\t{index.Path}={index.Value}";
            //}
        }
    }

    public class Foo
    {
        public int Score { get; set; }
        public int[] Scores { get; set; }
        public FooType Type { get; set; }
        public DateTime TimeStamp { get; set; }
        public int? OptScore { get; set; }
        public int?[] OptScores { get; set; }
        public string Name { get; set; }
        public string[] Names { get; set; }
        public bool Bool { get; set; }
        public bool OptBool { get; set; }
        //public Bar Bar { get; set; }
        //public KeyValuePair<int, string> Kv { get; set; }

        public static Foo CreateSample()
        {
            return new Foo
            {
                Score = 42,
                Scores = new[] { 1, 2, 3 },
                Type = FooType.One,
                OptScore = 111,
                OptScores = new int?[] { 111, 112, 113 },
                Bool = true,
                OptBool = true,
                TimeStamp = DateTime.Now,
                Name = "Test",
                Names = new[] { "Name1", "Name2" },
                //Kv = new KeyValuePair<int, string>(333, "aaa")
            };
        }
    }

    public enum FooType
    {
        One
    }

    public class Bar
    {
        public int[] BarScore { get; set; }
    }
}
