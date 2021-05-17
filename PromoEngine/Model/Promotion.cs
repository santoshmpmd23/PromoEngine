using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoEngine.Model
{
    public enum PromotionType
    {
        Individual,
        Combination
    }
    public class Promotion
    {
        public int PromotionId { get; set; }
        public int Quantity { get; set; }
        public PromotionType Type { get; set; }
        public List<string> ProductCode { get; set; }
        public double Price { get; set; }
    }
}
