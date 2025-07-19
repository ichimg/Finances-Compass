using DebtsCompass.Application.Exceptions;
using DebtsCompass.Domain.Entities.Requests;
using DebtsCompass.Domain.Interfaces;
using DebtsCompass.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace DebtsCompass.Presentation.Controllers
{
    [ApiController]
    [Route("api")]
    public class IncomesController : ControllerBase
    {
        private readonly IIncomesService incomesService;
        public IncomesController(IIncomesService incomesService)
        {
            this.incomesService = incomesService;
        }

        [HttpPost]
        [Route("create-income")]
        [Authorize]
        public async Task<ActionResult<Guid>> CreateIncome([FromBody] CreateIncomeRequest createIncomeRequest,
          [FromQuery(Name = "email")] string email)
        {
            Guid createdExpenseId = await incomesService.CreateIncome(createIncomeRequest, email);

            return Ok(new Response<object>
            {
                Message = null,
                Payload = createdExpenseId,
                StatusCode = HttpStatusCode.Created
            });
        }

        [HttpDelete]
        [Route("delete-income")]
        [Authorize]
        public async Task<ActionResult<object>> DeleteIncome([FromQuery] string id, [FromQuery] string email)
        {
            if (!IsRequestFromValidUser(email))
            {
                throw new ForbiddenRequestException();
            }

            await incomesService.DeleteIncome(id, email);

            return Ok(new Response<object>
            {
                Message = null,
                Payload = null,
                StatusCode = HttpStatusCode.OK
            });
        }

        [HttpPut]
        [Route("edit-income")]
        [Authorize]
        public async Task<ActionResult<object>> UpdateIncome([FromBody] EditIncomeRequest editIncomeRequest, [FromQuery(Name = "email")] string email)
        {
            if (!IsRequestFromValidUser(email))
            {
                throw new ForbiddenRequestException();
            }

            await incomesService.UpdateIncome(editIncomeRequest, email);

            return Ok(new Response<object>
            {
                Message = null,
                Payload = null,
                StatusCode = HttpStatusCode.OK
            });
        }

        private bool IsRequestFromValidUser(string email)
        {
            var userIdentity = User.Identity as ClaimsIdentity;
            var userEmailClaim = userIdentity.FindFirst(ClaimTypes.Email)?.Value;

            return string.Equals(userEmailClaim, email, StringComparison.OrdinalIgnoreCase);
        }
    }
}
