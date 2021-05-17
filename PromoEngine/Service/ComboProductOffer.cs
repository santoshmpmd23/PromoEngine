using Microsoft.Extensions.Logging;
using PromoEngine.Model;
using PromoEngine.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PromoEngine.Service
{
    public class ComboProductOffer : IProductOffer
    {
        readonly ILogger logger;
        readonly List<Promotion> promotions;

        public ComboProductOffer(ILogger<ComboProductOffer> _logger, IRepository _repo)
        {
            logger = _logger;
            promotions = _repo.GetAllProductOffers();
        }
        public bool CanApplyOffer(CheckoutProduct checkoutProduct, out Promotion appliedPromotion)
        {
            appliedPromotion = promotions.Where(x => x.Type == PromotionType.Combination)
                                         .Where(x => x.ProductCode.Contains(checkoutProduct.Product.ProductCode)).FirstOrDefault();
            if (appliedPromotion != null && !checkoutProduct.IsValidated)
            {
                return true;
            }
            return false;
        }

        public double GetOfferPrice(List<CheckoutProduct> productCheckoutList, Promotion appliedPromotion, CheckoutProduct checkoutProductCurrent)
        {
            List<CheckoutProduct> checkoutProducts = new List<CheckoutProduct>();
            double offerPrice = 0;
            productCheckoutList.ForEach(item =>
            {
                if (appliedPromotion.ProductCode.Contains(item.Product.ProductCode))
                {
                    if (item.Product.ProductCode == checkoutProductCurrent.Product.ProductCode)
                        item.IsValidated = true;
                    checkoutProducts.Add(item);
                }
            });
            try
            {
                if (checkoutProducts.Count < appliedPromotion.ProductCode.Count)
                    return checkoutProductCurrent.Product.Price;

                var eachProdQnty = appliedPromotion.Quantity / appliedPromotion.ProductCode.Count;
                var eachprodPrice = appliedPromotion.Price / appliedPromotion.ProductCode.Count;
                var freq = checkoutProductCurrent.Quantity / eachProdQnty;
                if (freq > 0)
                {
                    offerPrice = (checkoutProductCurrent.Product.Price * (checkoutProductCurrent.Quantity - (eachProdQnty * freq))) + (eachprodPrice * eachProdQnty * freq);
                }
                else
                    offerPrice = (checkoutProductCurrent.Product.Price * (checkoutProductCurrent.Quantity - eachProdQnty)) + (eachprodPrice * eachProdQnty);
            }
            catch (Exception ex)
            {
                logger.LogError("Error in ComboProductOffer :" + ex.Message);
            }

            return offerPrice;
        }
    }
}
