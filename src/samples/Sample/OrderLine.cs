using System.Collections.Generic;

namespace Sample
{
    public class OrderLine
    {
        public string ArticleNo { get; set; }
        public int Qty { get; set; }
        public List<Prop> Props { get; set; }
    }
}