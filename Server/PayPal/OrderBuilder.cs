using Mediwatch.Server.PayPal.Values;
using PayPalCheckoutSdk.Orders;
using System.Collections.Generic;

namespace Mediwatch.Server.PayPal
{
    public class OrderBuilder
    {
        /// <summary>
        /// Use classes from the PayPalCheckoutSdk to build an OrderRequest
        /// </summary>
        /// <returns></returns>
        public static OrderRequest Build()
        {
            // OrderRequest orderRequest = new OrderRequest()
            // {
            //     CheckoutPaymentIntent = CheckoutPaymentIntent.CAPTURE,
            //     ApplicationContext = new ApplicationContext
            //     {
            //         BrandName = "Delaneys.space",
            //         LandingPage = PayPal.Values.LandingPage.LOGIN,
            //         UserAction = PayPal.Values.UserAction.PAY_NOW,
            //         ShippingPreference = PayPal.Values.ShippingPreference.NO_SHIPPING,
            //         Locale = "fr-FR"
            //     },
            //     PurchaseUnits = new List<PurchaseUnitRequest>
            //     {
            //         new PurchaseUnitRequest
            //         {
            //             ReferenceId = "default",//modify
            //             Description = "Mediwatch formation",
            //             SoftDescriptor = "Mediwatch",
            //              AmountWithBreakdown = new AmountWithBreakdown
            //             {
            //                 CurrencyCode = PayPal.Values.CurrencyCode.EUR,
            //                 Value = "42.66",
            //                 // AmountBreakdown = new AmountBreakdown
            //                 // {
            //                 //     ItemTotal = new Money
            //                 //     {
            //                 //         CurrencyCode = basket.CurrencyCode,
            //                 //         Value = basket.SubTotal.ToString()
            //                 //     },
            //                 // }
            //             },
            //         }
            //     }
            // };

            // orderRequest.PurchaseUnits[0].Items.Add(new Item 
            // {
            //     Name = "Formation PLACEHOLDER",
            //     Description = "This is a PLACEHOLDER",
            //     UnitAmount = new Money {
            //          CurrencyCode = PayPal.Values.CurrencyCode.EUR,
            //          Value = "42.66"
            //     },
            //     Quantity = "1",
            //     Category = PayPal.Values.Items.Category.DIGITAL_GOODS
            // });




            //Panier implémentation
            // foreach (var product in basket.Products)
            // {
            //     orderRequest.PurchaseUnits[0]
            //                 .Items
            //                 .Add(new Item
            //                 {
            //                     Name = product.Name,
            //                     Description = product.Description,
            //                     UnitAmount = new Money
            //                     {
            //                         CurrencyCode = basket.CurrencyCode,
            //                         Value = product.Price.ToString()
            //                     },
            //                     Quantity = product.Quantity.ToString(),
            //                     Category = PayPal.Values.Item.Category.DIGITAL_GOODS
            //                 });
            // }
            var order = new OrderRequest()
            {
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new List<PurchaseUnitRequest>()
                {
                    new PurchaseUnitRequest()
                    {
                        AmountWithBreakdown = new AmountWithBreakdown()
                        {
                            CurrencyCode = "USD",
                            Value = "100.00"
                        }
                    }
                }
            };
            return order;
        }
    }
}