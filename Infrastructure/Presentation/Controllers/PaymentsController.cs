using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceAbstraction;
using Shared.DataTransferObjects.BasketModule;
using Stripe;

namespace Presentation.Controllers
{
    public class PaymentsController(IServiceManager _serviceManager, ILogger<PaymentsController> _logger) : ApiBaseController
    {
        private const string whSecret = "whsec_XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _serviceManager.PaymentService.CreateOrUpdatePaymentIntentAsync(basketId);
            return Ok(basket);
        }

        //Webhook
        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], whSecret);

            var intent = (PaymentIntent)stripeEvent.Data.Object;

            string? result;

            switch (stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                    result = await _serviceManager.PaymentService.UpdateOrderStatusAsync(intent.Id, true);
                    _logger.LogInformation("Order has succeeded {0}", result);
                    _logger.LogInformation("Unhandled event type : {0}", stripeEvent.Type);
                    break;
                case "payment_intent.payment_failed":
                    result = await _serviceManager.PaymentService.UpdateOrderStatusAsync(intent.Id, false);
                    _logger.LogInformation("Order has Failed {0}", result);
                    _logger.LogInformation("Unhandled event type : {0}", stripeEvent.Type);
                    break;
            }

            return Ok();
        }
    }
}
