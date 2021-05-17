using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoEngine.Model
{
    public class CheckoutProduct
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public double CheckoutPrice { get; set; }
        public bool IsValidated { get; set; }
    }
}
