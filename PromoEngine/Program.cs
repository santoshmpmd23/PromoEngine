using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using PromoEngine.Repository;
using PromoEngine.Service;
using System;
using System.Linq;

namespace PromoEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                PromoEngineClient client = serviceProvider.GetService<PromoEngineClient>();

                client.ScenarioA();

                client.ScenarioB();

                client.ScenarioC();
            }

            Console.WriteLine("Done");
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Information);
                builder.AddNLog("nlog.config");
            });

            services.AddSingleton<IRepository, JsonRepository>();
            services.AddSingleton<IProductOffer, SingleProductOffer>();
            services.AddSingleton<IProductOffer, ComboProductOffer>();
            services.AddSingleton<PromotionService>(x =>
            {
                var loggerFactory = services.BuildServiceProvider().GetService<ILoggerFactory>();
                var logger = new Logger<PromotionService>(loggerFactory);
                var offrList = services.BuildServiceProvider().GetServices<IProductOffer>();
                return new PromotionService(logger, offrList.ToList());
            });
            services.AddSingleton<PromoEngineClient>(x =>
            {
                var svc = services.BuildServiceProvider().GetService<PromotionService>();
                var repo = services.BuildServiceProvider().GetService<IRepository>();
                return new PromoEngineClient(svc, repo);
            });
        }
    }
}
