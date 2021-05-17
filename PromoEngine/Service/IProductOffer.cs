using PromoEngine.Model;
using System.Collections.Generic;

namespace PromoEngine.Service
{
    public interface IProductOffer
    {
        bool CanApplyOffer(CheckoutProduct checkoutProduct, out Promotion appliedPromotion);

        double GetOfferPrice(List<CheckoutProduct> productCheckoutList, Promotion appliedPromotion, CheckoutProduct checkoutProductCurrent);
    }
}
