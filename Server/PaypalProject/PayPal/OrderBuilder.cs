using PayPalCheckoutSdk.Orders;
using System.Collections.Generic;

namespace PayPal
{
    public static class OrderBuilder
    {
        /// <summary>
        /// Use classes from the PayPalCheckoutSdk to build an OrderRequest
        /// </summary>
        /// <returns></returns>
        public static OrderRequest Build(Services.Data.Core.IUnitOfWork unitOfWork)
        {

            var formation = unitOfWork.FormationRepository.SingleOrDefault(x => x.Id == x.Id);

            if (formation == null)
                return null;

            //https://developer.paypal.com/docs/api/reference/locale-codes/#


            OrderRequest orderRequest = new OrderRequest()
            {
                CheckoutPaymentIntent = PayPal.Values.CheckoutPaymentIntent.CAPTURE,
                ApplicationContext = new ApplicationContext
                {
                    BrandName = "Mediwatch.space",
                    LandingPage = PayPal.Values.LandingPage.LOGIN,
                    UserAction = PayPal.Values.UserAction.PAY_NOW,
                    ShippingPreference = PayPal.Values.ShippingPreference.NO_SHIPPING,
                    Locale = "fr-FR"
                },
                PurchaseUnits = new List<PurchaseUnitRequest>
                {
                    new PurchaseUnitRequest
                    {
                        ReferenceId = "Mediwatch.space", // [required] The merchant ID for the purchase unit.
                        Description = "Software published by Mediwatch.space",
                        SoftDescriptor = "Mediwatch.space",
                        AmountWithBreakdown = new AmountWithBreakdown
                        {
                            CurrencyCode = PayPal.Values.CurrencyCode.EUR,
                            Value = formation.Total.ToString(),
                            AmountBreakdown = new AmountBreakdown
                            {
                                ItemTotal = new Money
                                {
                                    CurrencyCode = formation.CurrencyCode,
                                    Value = formation.SubTotal.ToString()
                                },
                                Discount = new Money
                                {
                                    CurrencyCode = formation.CurrencyCode,
                                    Value = formation.Discount.ToString()
                                }
                            }
                        },
                        Items = new List<Item>()
                    }
                }
            };

            foreach (var product in formation.Products)
            {
                orderRequest.PurchaseUnits[0]
                            .Items
                            .Add(new Item
                            {
                                Name = product.Name,
                                Description = product.Description,
                                UnitAmount = new Money
                                {
                                    CurrencyCode = formation.CurrencyCode,
                                    Value = product.Price.ToString()
                                },
                                Quantity = product.Quantity.ToString(),
                                Category = PayPal.Values.Item.Category.DIGITAL_GOODS
                            });
            }


            return orderRequest;
        }
    }
}