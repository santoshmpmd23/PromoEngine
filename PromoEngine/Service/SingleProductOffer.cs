using Microsoft.Extensions.Logging;
using PromoEngine.Model;
using PromoEngine.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PromoEngine.Service
{
    public class SingleProductOffer : IProductOffer
    {
        readonly ILogger logger;
        readonly List<Promotion> promotions;

        
        public SingleProductOffer(ILogger<SingleProductOffer> _logger, IRepository _repo)
        {
            logger = _logger;
            promotions = _repo.GetAllProductOffers();
        }

        public bool CanApplyOffer(CheckoutProduct checkoutProduct, out Promotion appliedPromotion)
        {
            appliedPromotion = promotions.Where(x=> x.Type== PromotionType.Individual)
                                         .Where(x => x.ProductCode.FirstOrDefault() == checkoutProduct.Product.ProductCode)
                                         .FirstOrDefault();
            if (appliedPromotion != null && !checkoutProduct.IsValidated)
            {
                checkoutProduct.IsValidated = true;
                return true;
            }

            return false;
        }

        public double GetOfferPrice(List<CheckoutProduct> productCheckoutList, Promotion appliedPromotion, CheckoutProduct checkoutProductCurrent)
        {
            double offerPrice = 0;
            try
            {
                int totalEligibleItems = checkoutProductCurrent.Quantity / appliedPromotion.Quantity;
                int remainingItems = checkoutProductCurrent.Quantity % appliedPromotion.Quantity;
                offerPrice = appliedPromotion.Price * totalEligibleItems + remainingItems * (checkoutProductCurrent.Product.Price);

            }
            catch (Exception e)
            {
                logger.LogError("Error in SingleProductOffer :" + e.Message);
            }

            return offerPrice;
        }
    }
}
