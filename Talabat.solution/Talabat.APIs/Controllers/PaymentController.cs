using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.Errors;
using Talabat.Core.Entites;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PaymentController : ApiBaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger _logger;
        private const string endpointSecret = "whsec_a5176311a8616c0f7e5d204eadf06b691d1c75b90ffe50231ee893d17dfa8567";


        public PaymentController(IPaymentService paymentService,
            ILogger logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> SetPaymentIntent(string basketId)
        {
            var basket = await _paymentService.SetPaymentIntent(basketId);

            return basket is null ? BadRequest(new ApiResponse(400, "an Error With ur Basket")) : Ok(basket);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], endpointSecret);

            var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
            // Handle the event
            switch (stripeEvent.Type)
            {
                case Events.PaymentIntentPaymentFailed:
                    await _paymentService.UpdatePaymentStatus(paymentIntent.Id, true);
                    _logger.LogInformation("Payment Succedded", paymentIntent.Id);
                    break;

                case Events.PaymentIntentSucceeded:
                    await _paymentService.UpdatePaymentStatus(paymentIntent.Id, false);
                    _logger.LogInformation("Payment Failed", paymentIntent.Id);
                    break;
            }

            return Ok();
        }
    }
}
