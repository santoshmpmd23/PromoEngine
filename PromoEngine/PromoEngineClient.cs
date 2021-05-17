using PromoEngine.Repository;
using PromoEngine.Service;
using PromoEngine.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PromoEngine
{
    public class PromoEngineClient
    {
        readonly PromotionService promotionService;
        readonly IRepository repo;
        readonly List<Product> prodsList;

        List<CheckoutProduct> checkoutList;
        public PromoEngineClient(PromotionService _promotionService, IRepository _repo)
        {
            promotionService = _promotionService;
            repo = _repo;
            prodsList = repo.GetAllAvilableProducts();
          
        }

        public void ScenarioA()
        {
            Console.WriteLine("Scenario A");
            checkoutList = new List<CheckoutProduct>()
            {
                new CheckoutProduct(){ Product= prodsList.FirstOrDefault(p=>p.ProductCode=="A"),Quantity=1 },
                new CheckoutProduct(){ Product= prodsList.FirstOrDefault(p=>p.ProductCode=="B"),Quantity=1 },
                new CheckoutProduct(){ Product= prodsList.FirstOrDefault(p=>p.ProductCode=="C"),Quantity=1 }
            };

            ApplyPromotion();
        }
        public void ScenarioB()
        {
            Console.WriteLine("Scenario B");
            checkoutList = new List<CheckoutProduct>()
            {
                new CheckoutProduct(){ Product= prodsList.FirstOrDefault(p=>p.ProductCode=="A"),Quantity=5 },
                new CheckoutProduct(){ Product= prodsList.FirstOrDefault(p=>p.ProductCode=="B"),Quantity=5 },
                new CheckoutProduct(){ Product= prodsList.FirstOrDefault(p=>p.ProductCode=="C"),Quantity=1 }
            };

            ApplyPromotion();
        }
        public void ScenarioC()
        {
            Console.WriteLine("Scenario C");
            checkoutList = new List<CheckoutProduct>()
            {
                new CheckoutProduct(){ Product= prodsList.FirstOrDefault(p=>p.ProductCode=="A"),Quantity=3 },
                new CheckoutProduct(){ Product= prodsList.FirstOrDefault(p=>p.ProductCode=="B"),Quantity=5 },
                new CheckoutProduct(){ Product= prodsList.FirstOrDefault(p=>p.ProductCode=="C"),Quantity=2 },
                new CheckoutProduct(){ Product= prodsList.FirstOrDefault(p=>p.ProductCode=="D"),Quantity=1 }
            };

            ApplyPromotion();
        }

        void ApplyPromotion()
        {
            var totaolPrice = promotionService.ApplyPromotion(checkoutList);
            Console.WriteLine("Checkout Products");
            checkoutList.ForEach(item =>
            {
                Console.WriteLine($"{item.Product.ProductCode} * {item.Quantity}");
            });
            Console.WriteLine($"Total Price ::: {totaolPrice}\n\n");
        }
    }
}
