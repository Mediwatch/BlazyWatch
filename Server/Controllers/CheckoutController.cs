using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using PayPalCheckoutSdk.Orders;
using Server;
using System;

namespace Mediwatch.Server.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly DbContextMediwatch _context;
        public CheckoutController(DbContextMediwatch context)
        {
            _context = context;
        }
        public class localBody
        {
            public string formationId { get; set; }
            // public string orderID { get; set; }
        }

        /// <summary>
        /// This action is called when the user clicks on the PayPal button.
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/paypal/checkout/order/create")]
        public async Task<PayPal.SmartButtonHttpResponse> Create([FromBody] localBody body)
        {
            FormationController _formationCtr = new FormationController(_context);
            var formFind = _formationCtr.GetFormation(Guid.Parse(body.formationId)).Result.Value;

            var request = new PayPalCheckoutSdk.Orders.OrdersCreateRequest();
            request.Prefer("return=representation");
            // request.RequestBody(PayPal.OrderBuilder.Build(formFind));
            request.RequestBody(PayPal.OrderBuilder.Build());
            // Call PayPal to set up a transaction
            var response = await PayPal.PayPalClient.Client().Execute(request);
            // Create a response, with an order id.
            var result = response.Result<PayPalCheckoutSdk.Orders.Order>();
            var payPalHttpResponse = new PayPal.SmartButtonHttpResponse(response)
            {
                orderID = result.Id
            };
            return payPalHttpResponse;
        }


        /// <summary>
        /// This action is called once the PayPal transaction is approved
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [Route("api/paypal/checkout/order/approved/{orderId}")]
        public IActionResult Approved(string orderId)
        {
            return Ok();
        }


        /// <summary>
        /// This action is called once the PayPal transaction is complete
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [Route("api/paypal/checkout/order/complete/{orderId}")]
        public IActionResult Complete(string orderId)
        {
            // 1. Update the database.
            // 2. Complete the order process. Create and send invoices etc.
            // 3. Complete the shipping process.
            System.Console.WriteLine(
            @"+++++++++++++++++++++++++++++++++++++++++++++++++++++\n" +
            orderId +
            @"+++++++++++++++++++++++++++++++++++++++++++++++++++++\n");
            return Ok();
        }


        /// <summary>
        /// This action is called once the PayPal transaction is complete
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [Route("api/paypal/checkout/order/cancel/{orderId}")]
        public IActionResult Cancel(string orderId)
        {
            // 1. Remove the orderId from the database.
            return Ok();
        }


        /// <summary>
        /// This action is called once the PayPal transaction is complete
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [Route("api/paypal/checkout/order/error/{orderId}/{error}")]
        public IActionResult Error(string orderId,
                                   string error)
        {
            // Log the error.
            System.Console.WriteLine(error);
            // Notify the user.
            return NoContent();
        }


    }
}