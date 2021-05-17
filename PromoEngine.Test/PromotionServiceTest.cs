using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PromoEngine.Service;
using PromoEngine.Model;
using PromoEngine.Repository;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace PromoEngine.Test
{
    [TestClass]
    public class PromotionServiceTest
    {
        Mock<ILogger<JsonRepository>> repoLogger;
        Mock<ILogger<PromotionService>> promoLogger;
        Mock<List<IProductOffer>> offerList;
        Mock<ILogger<SingleProductOffer>> singleProdLogger;
        Mock<ILogger<ComboProductOffer>> comboProdLogger;

        PromotionService promotionService;
        List<CheckoutProduct> checkoutList;
        List<Product> prodsList;

        IRepository repo;
        [TestInitialize]
        public void SetUp()
        {
            repoLogger = new Mock<ILogger<JsonRepository>>();
            repo = new JsonRepository(repoLogger.Object);
            singleProdLogger = new Mock<ILogger<SingleProductOffer>>();
            comboProdLogger = new Mock<ILogger<ComboProductOffer>>();
            offerList = new Mock<List<IProductOffer>>();
            offerList.Object.Add(new SingleProductOffer(singleProdLogger.Object, repo));
            offerList.Object.Add(new ComboProductOffer(comboProdLogger.Object, repo));
            promoLogger = new Mock<ILogger<PromotionService>>();
            prodsList = repo.GetAllAvilableProducts();

            //var temp = new Mock<PromotionService>().Setup(x => x.ApplyPromotion(It.IsAny<List<CheckoutProduct>>())).Returns(100);
        }

        [TestMethod]
        public void ScenarioA()
        {
            //setup data
            checkoutList = new List<CheckoutProduct>()
            {
                new CheckoutProduct(){ Product= prodsList.FirstOrDefault(p=>p.ProductCode=="A"),Quantity=1 },
                new CheckoutProduct(){ Product= prodsList.FirstOrDefault(p=>p.ProductCode=="B"),Quantity=1 },
                new CheckoutProduct(){ Product= prodsList.FirstOrDefault(p=>p.ProductCode=="C"),Quantity=1 }
            };
            double expectedResult = 100;

            //Act           
            promotionService = new PromotionService(promoLogger.Object, offerList.Object);
            var actualResult = promotionService.ApplyPromotion(checkoutList);
            //Assert
            Assert.AreEqual(expectedResult, actualResult, "Scenario A Failed");
        }

        [TestMethod]
        public void ScenarioB()
        {
            //setup data
            checkoutList = new List<CheckoutProduct>()
            {
                new CheckoutProduct(){ Product= prodsList.FirstOrDefault(p=>p.ProductCode=="A"),Quantity=5 },
                new CheckoutProduct(){ Product= prodsList.FirstOrDefault(p=>p.ProductCode=="B"),Quantity=5 },
                new CheckoutProduct(){ Product= prodsList.FirstOrDefault(p=>p.ProductCode=="C"),Quantity=1 }
            };
            double expectedResult = 370;

            //Act           
            promotionService = new PromotionService(promoLogger.Object, offerList.Object);
            var actualResult = promotionService.ApplyPromotion(checkoutList);
            //Assert
            Assert.AreEqual(expectedResult, actualResult, "Scenario B Failed");
        }

        [TestMethod]
        public void ScenarioC()
        {
            //setup data
            checkoutList = new List<CheckoutProduct>()
            {
                new CheckoutProduct(){ Product= prodsList.FirstOrDefault(p=>p.ProductCode=="A"),Quantity=3 },
                new CheckoutProduct(){ Product= prodsList.FirstOrDefault(p=>p.ProductCode=="B"),Quantity=5 },
                new CheckoutProduct(){ Product= prodsList.FirstOrDefault(p=>p.ProductCode=="C"),Quantity=1 },
                new CheckoutProduct(){ Product= prodsList.FirstOrDefault(p=>p.ProductCode=="D"),Quantity=1 }
            };
            double expectedResult = 280;

            //Act           
            promotionService = new PromotionService(promoLogger.Object, offerList.Object);
            var actualResult = promotionService.ApplyPromotion(checkoutList);
            //Assert
            Assert.AreEqual(expectedResult, actualResult, "Scenario C Failed");
        }
    }
}
