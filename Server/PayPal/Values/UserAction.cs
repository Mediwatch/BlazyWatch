using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mediwatch.Server.PayPal.Values
{
    public static class UserAction
    {
        /// <summary>
        /// After you redirect the customer to the PayPal payment page,
        /// a Continue button appears. Use this option when the final amount is not known
        /// when the checkout flow is initiated and you want to redirect
        /// the customer to the merchant page without processing the payment.
        /// </summary>
        public static string CONTINUE { get; private set; } = "CONTINUE";

        /// <summary>
        /// After you redirect the customer to the PayPal payment page,
        /// a Pay Now button appears.
        /// Use this option when the final amount is known when the checkout is initiated
        /// and you want to process the payment immediately when the customer clicks Pay Now.
        /// </summary>
        public static string PAY_NOW { get; private set; } = "PAY_NOW";
    }
}