using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Service.Services.BasketService.Dtos;
using Store.Service.Services.PaymentService;
using Stripe;

namespace Store.Web.Controllers
{
    public class PaymentController : BasicsController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;
        const string endpointSecret = "whsec_294cea4f3a1e1851177776b1157062178c94849dc45abe9c3fd46b4a02cf7a5b";
        public PaymentController(IPaymentService paymentService,ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

      [HttpPost]
      public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntentAsync(CustomerBasketDto basket)
            =>Ok(await _paymentService.CreateOrUpdatePaymentIntentAsync(basket));


        [HttpPost]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);

                PaymentIntent paymentIntent;
                // Handle the event
                if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
                {
                    paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    _logger.LogInformation("Payment Failed : ", paymentIntent.Id);
                    await _paymentService.UpdateOrderPaymentSucceeded(paymentIntent.Id);
                }
                else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
                {
                    paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    _logger.LogInformation("Payment Failed : ", paymentIntent.Id);
                    await _paymentService.UpdateOrderPaymentFailed(paymentIntent.Id);
                }
                else if(stripeEvent.Type == EventTypes.PaymentIntentCreated)
                {
                    _logger.LogInformation("Payment Created");
                }
                // ... handle other event types
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
