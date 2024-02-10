using DebtsCompass.Application.Exceptions;
using DebtsCompass.Domain;
using DebtsCompass.Domain.Entities.Requests;
using DebtsCompass.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using System.Net;

namespace DebtsCompass.Presentation.Controllers
{
    [ApiController]
    [Route("api")]
    public class PaypalController : ControllerBase
    {

        private readonly IPaypalService paypalService;
        private readonly IDebtsService debtsService;
        public PaypalController(IPaypalService paypalService, IDebtsService debtsService)
        {
            this.paypalService = paypalService;
            this.debtsService = debtsService;
        }

        [HttpGet("access-token")]
        public async Task<ActionResult<string>> GetAccessToken()
        {
            string accessToken = await paypalService.GetAccessToken();
            return Ok(accessToken);
        }

        [HttpPost("create-paypal-order")]
        public async Task<ActionResult<Response<string>>> CreateOrder([FromBody] CreatePaypalOrderRequest createOrderRequest)
        {
            var response = await paypalService.CreateOrder(createOrderRequest);

            return Ok(new Response<string>
            {
                Message = null,
                Payload = response,
                StatusCode = HttpStatusCode.OK
            });
        }

        [HttpPost("complete-paypal-order")]
        public async Task<ActionResult<Response<string>>> CompleteOrder([FromBody] CompletePaypalOrderRequest completeOrderRequest)
        {
            var response = await paypalService.CompleteOrder(completeOrderRequest);
            await debtsService.PayDebt(completeOrderRequest.DebtId);

            return Ok(new Response<string>
            {
                Message = null,
                Payload = response,
                StatusCode = HttpStatusCode.OK
            });
        }
    }
}
