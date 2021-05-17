using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PromoEngine.Model;
namespace PromoEngine.Repository
{
    public interface IRepository
    {
      
        List<Product> GetAllAvilableProducts();

       
        List<Promotion> GetAllProductOffers();
    }
}
