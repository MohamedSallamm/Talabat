using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.API.DTOs;
using Talabat.API.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Services.Contract;

namespace Talabat.API.Controllers
{
    [Authorize]
    public class PaymentsController : APIBaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentsController> _logger;
        private const string _WHsecret = "sk_test_51PuHqaRxZPNdZJJcB6q1PJr0hFy0ZVsy74m7mIj2SyUrKF0aOa8uea0fkbnDMFPQRjA9QTAlZZJoG6UFOjrPx5nc007S7pkuUN";

        public PaymentsController(IPaymentService paymentService, IMapper mapper, ILogger<PaymentsController> logger, IConfiguration config)
        {
            _paymentService = paymentService;
            _mapper = mapper;
            _logger = logger;

        }

        // ✅ إنشاء أو تحديث Payment Intent
        [ProducesResponseType(typeof(CustomerBasketDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketDTO>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var customerBasket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (customerBasket is null)
                return BadRequest(new APIResponse(400, "The Basket is Empty"));

            var mapped = _mapper.Map<CustomerBasket, CustomerBasketDTO>(customerBasket);
            return Ok(mapped);
        }

        [HttpPost("Webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var signatureHeader = Request.Headers["Stripe-Signature"];

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, signatureHeader, _WHsecret);

                switch (stripeEvent.Type)
                {
                    case EventTypes.PaymentIntentSucceeded:
                        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                        _logger.LogInformation("✅ دفع ناجح بمبلغ: {Amount}", paymentIntent?.Amount);
                        if (paymentIntent != null)
                        {
                            await _paymentService.UpdatePaymentIntentToSucceededOrFailed(paymentIntent.Id, paymentIntent.Status == "succeeded");
                        }
                        break;

                    case EventTypes.PaymentIntentPaymentFailed:
                        var paymentIntentFailed = stripeEvent.Data.Object as PaymentIntent;
                        _logger.LogWarning("❌ فشل الدفع بمبلغ: {Amount}", paymentIntentFailed?.Amount);
                        if (paymentIntentFailed != null)
                        {
                            await _paymentService.UpdatePaymentIntentToSucceededOrFailed(paymentIntentFailed.Id, paymentIntentFailed.Status == "requires_payment_method");
                        }
                        break;

                    default:
                        _logger.LogWarning("⚠️ حدث غير معروف: {EventType}", stripeEvent.Type);
                        break;
                }

                return Ok();
            }
            catch (StripeException e)
            {
                _logger.LogError("❌ خطأ في Webhook: {Message}", e.Message);
                return BadRequest();
            }
        }

    }
}
