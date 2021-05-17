using Microsoft.Extensions.Logging;
using PromoEngine.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PromoEngine.Service
{
    public class PromotionService
    {
        readonly ILogger logger;
        List<IProductOffer> offerList;

        public PromotionService(ILogger<PromotionService> _logger, List<IProductOffer> _offerList)
        {
            logger = _logger;
            offerList = _offerList;
            logger.LogInformation("Initilized PromotionService.");
        }

        public double ApplyPromotion(List<CheckoutProduct> checkoutList)
        {
            double totalPrice = 0;
            try
            {
                foreach (CheckoutProduct item in checkoutList.Where(l=> l.Quantity>0))
                {
                        foreach (var offer in offerList)
                        {
                        if (offer.CanApplyOffer(item, out Promotion appliedPromotion))
                        {
                            item.CheckoutPrice = offer.GetOfferPrice(checkoutList, appliedPromotion, item);
                            totalPrice += item.CheckoutPrice;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Applying Promotion in PromotionStrategy:" + ex.Message);
            }
            return totalPrice;
        }

    }
}
