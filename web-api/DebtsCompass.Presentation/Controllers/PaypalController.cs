using DebtsCompass.Application.Exceptions;
using DebtsCompass.Domain;
using DebtsCompass.Domain.Entities.Requests;
using DebtsCompass.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

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
        [Authorize]
        public async Task<ActionResult<string>> GetAccessToken()
        {
            string accessToken = await paypalService.GetAccessToken();
            return Ok(accessToken);
        }

        [HttpPost("create-paypal-order")]
        [Authorize]
        public async Task<ActionResult<Response<string>>> CreateOrder([FromBody] CreatePaypalOrderRequest createOrderRequest)
        {
            var userIdentity = User.Identity as ClaimsIdentity;
            var userEmailClaim = userIdentity.FindFirst(ClaimTypes.Email)?.Value;

            var response = await paypalService.CreateOrder(createOrderRequest, userEmailClaim);

            return Ok(new Response<string>
            {
                Message = null,
                Payload = response,
                StatusCode = HttpStatusCode.OK
            });
        }

        [HttpPost("complete-paypal-order")]
        [Authorize]
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
 