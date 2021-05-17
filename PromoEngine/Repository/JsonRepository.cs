using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PromoEngine.Constants;
using PromoEngine.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PromoEngine.Repository
{
    public class JsonRepository : IRepository
    {
        readonly ILogger logger;
        readonly IConfiguration configuration;
        public JsonRepository(ILogger<JsonRepository> _logger)
        {
            logger = _logger;
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile(Constant.ProductsAndPromotionData, false);

            configuration = builder.Build();
        }

        public List<Product> GetAllAvilableProducts()
        {
            List<Product> productList = new();
            configuration.GetSection(Constant.Products).GetChildren().ToList().ForEach((item) =>
            {
                Product product = new();
                configuration.GetSection(item.Path).Bind(product);
                productList.Add(product);
            });
            logger.LogInformation($"Loaded {productList.Count} Products from json");
            return productList;
        }

        public List<Promotion> GetAllProductOffers()
        {
            List<Promotion> promoList = new();
            configuration.GetSection(Constant.Promotions).GetChildren().ToList().ForEach((item) =>
            {
                Promotion product = new();
                configuration.GetSection(item.Path).Bind(product);
                promoList.Add(product);
            });
            logger.LogInformation($"Loaded {promoList.Count} Promotion from json");
            return promoList;
        }

       
    }
}
