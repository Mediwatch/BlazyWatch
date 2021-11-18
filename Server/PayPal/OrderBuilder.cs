using Mediwatch.Server.PayPal.Values;
using Mediwatch.Shared.Models;
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
        public static OrderRequest Build(formation currentForm)
        {
            #region testRegion
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
            #endregion

            string priceStr = FormatPrice(currentForm.Price);
            var order = new OrderRequest()
            {
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new List<PurchaseUnitRequest>()
                {
                    new PurchaseUnitRequest()
                    {
                        AmountWithBreakdown = new AmountWithBreakdown()
                        {
                            CurrencyCode = PayPal.Values.CurrencyCode.EUR,
                            Value = priceStr
                        }
                    }
                }
            };
            return order;
        }

        public static OrderRequest Build(List<formation> listCurrentForm)
        {
            var priceGet = GetPrice(listCurrentForm);
            var order = new OrderRequest()
            {
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new List<PurchaseUnitRequest>()
                {
                    new PurchaseUnitRequest()
                    {
                        // AmountWithBreakdown = new AmountWithBreakdown()
                        // {
                        //     CurrencyCode = PayPal.Values.CurrencyCode.EUR,
                        //     Value = priceGet
                        // }
                        AmountWithBreakdown = new AmountWithBreakdown()
                        {
                            CurrencyCode = PayPal.Values.CurrencyCode.EUR,
                            Value = priceGet,
                            AmountBreakdown = new AmountBreakdown
                            {
                                ItemTotal = new Money
                                {
                                    CurrencyCode = PayPal.Values.CurrencyCode.EUR,
                                    Value = priceGet
                                }
                            },
                        },
                        Items = GetItems(listCurrentForm)
                    }
                }
            };
            return order;
        }

        private static List<Item> GetItems(List<formation> listCurrentForm)
        {
            var ListItems = new List<Item>();
            foreach (var itForm in listCurrentForm)
            {
                var newItem = new Item(){
                    Name = itForm.Name,
                    Quantity = "1",
                    Description = itForm.Description
                };
            }
            return ListItems;
        }
        private static string FormatPrice(decimal input)
        {
            return input.ToString().Replace(",", ".");
        }
        private static string GetPrice(List<formation> listCurrentForm)
        {
            decimal TotalPrice = 0;
            foreach (var curForm in listCurrentForm)
            {
                TotalPrice += curForm.Price;
            }
            return FormatPrice(TotalPrice);
        }
    }
}