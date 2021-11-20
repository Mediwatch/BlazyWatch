using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using PayPalCheckoutSdk.Orders;
using Server;
using System;
using Mediwatch.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Mediwatch.Server.Areas.Identity.Data;
using System.Collections.Generic;
using Mediwatch.Server.Controllers;
using Services.Data.Persistence;
using PayPal;

namespace Controllers
{
    public class CheckoutController : Controller
    {
        private readonly DbContextMediwatch _context;
        private IConfiguration _configuration;
        private readonly UserManager<UserCustom> userManager;

        public CheckoutController(DbContextMediwatch context,
        IConfiguration configuration,
        UserManager<UserCustom> _userManager)
        {
            _context = context;
            _configuration = configuration;
            this.userManager = _userManager;

        }
        public class localBody
        {
            public List<string> formationId { get; set; }
            public string orderId { get; set; }
        }

        /// <summary>
        /// This action is called when the user clicks on the PayPal button.
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/paypal/checkout/order/create")]
        public async Task<PayPal.SmartButtonHttpResponse> Create([FromBody] localBody body)
        {
            // AcountCountroller
            // Console.WriteLine((await userManager.FindByNameAsync(User.Identity.Name)).Id);
            Guid idUserDB = (await userManager.FindByNameAsync(User.Identity.Name)).Id;
            FormationController _formationCtr = new FormationController(_context);
            ApplicantSessionController _appSessionController = new ApplicantSessionController(_context);

            // var ListFormFind = new List<formation>();
            // foreach (var it in body.formationId)
            // {
            //     Guid.TryParse(it, out Guid id_Formation);
            //     ListFormFind.Add(_formationCtr.GetFormation(id_Formation).Result.Value);
            // }


            var unitTest = new UnitOfWork()
            {
                
            };
                OrdersCreateRequest request = new PayPalCheckoutSdk.Orders.OrdersCreateRequest();
                request.Prefer("return=representation");
                request.RequestBody(PayPal.OrderBuilder.Build(unitTest));




                // Call PayPal to set up a transaction
                PayPal.PayPalClient.LiveClientId = _configuration["Authentication:PayPal:LiveClientId"];
                PayPal.PayPalClient.LiveClientSecret = _configuration["Authentication:PayPal:LiveClientSecret"];
                PayPal.PayPalClient.SandboxClientId = _configuration["Authentication:PayPal:SandboxClientId"];
                PayPal.PayPalClient.SandboxClientSecret = _configuration["Authentication:PayPal:SandboxClientSecret"];



                var response = await PayPal.PayPalClient.Client().Execute(request);
                // Create a response, with an order id.
                var result = response.Result<PayPalCheckoutSdk.Orders.Order>();
                var payPalHttpResponse = new PayPal.SmartButtonHttpResponse(response)
                {
                    orderID = result.Id
                };
                applicant_session applicantSession = new applicant_session()
                {
                    idUser = idUserDB,
                    // idFormation = formFind.id,
                    confirmed = false,
                    payed = false,
                    idPayPal = result.Id
                };
                await _appSessionController.PostApplicantSession(applicantSession);
                return payPalHttpResponse;
        }


        /// <summary>
        /// This action is called once the PayPal transaction is approved
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("api/paypal/checkout/order/approved/{orderId}")]
        public IActionResult Approved(string orderId)
        {
            return Ok();
        }


        /// <summary>
        /// This action is called once the PayPal transaction is complete
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        // [HttpPost("api/paypal/checkout/order/complete/{orderId}")]
        // public IActionResult Complete(string orderId, [FromBody] localBody body)
        // {
        //     // 1. Update the database.
        //     // 2. Complete the order process. Create and send invoices etc.
        //     // 3. Complete the shipping process.
        //     System.Console.WriteLine(
        //     @"+++++++++++++++++++++++++++++++++++++++++++++++++++++\n" +
        //     body.formationId +
        //     @"+++++++++++++++++++++++++++++++++++++++++++++++++++++\n");
        //     return Ok();
        // }

        [HttpGet("api/paypal/checkout/order/complete/{orderId}")]
        public IActionResult Complete(string orderId)
        {
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
        [HttpGet("api/paypal/checkout/order/cancel/{orderId}")]
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
        [HttpGet("api/paypal/checkout/order/error/{orderId}/{error}")]
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