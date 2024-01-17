using DebtsCompass.Application.Exceptions;
using DebtsCompass.Domain;
using DebtsCompass.Domain.Entities.DtoResponses;
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
    public class DebtsController : ControllerBase
    {
        private readonly IDebtsService debtsService;
        public DebtsController(IDebtsService debtsService)
        {
            this.debtsService = debtsService;
        }

        [HttpGet]
        [Route("view-receiving-debts")]
        [Authorize]
        public async Task<ActionResult<List<DebtDto>>> GetReceivingDebts([FromHeader] string email)
        {
            if (!IsRequestFromValidUser(email))
            {
                throw new ForbiddenRequestException();
            }

            var debts = await debtsService.GetAllReceivingDebts(email);

            Response<List<DebtDto>> response = new Response<List<DebtDto>>
            {
                Message = null,
                Payload = debts,
                StatusCode = HttpStatusCode.OK
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("view-user-debts")]
        [Authorize]
        public async Task<ActionResult<List<DebtDto>>> GetUserDebts([FromHeader] string email)
        {
            if (!IsRequestFromValidUser(email))
            {
                throw new ForbiddenRequestException();
            }

            var debts = await debtsService.GetAllUserDebts(email);

            Response<List<DebtDto>> response = new Response<List<DebtDto>>
            {
                Message = null,
                Payload = debts,
                StatusCode = HttpStatusCode.OK
            };

            return Ok(response);
        }

        [HttpPost]
        [Route("create-debt")]
        [Authorize]
        public async Task<ActionResult<object>> CreateDebt([FromBody] CreateDebtRequest createDebtRequest,
            [FromQuery(Name = "email")] string email)
        {
            if (!IsRequestFromValidUser(email))
            {
                throw new ForbiddenRequestException();
            }

            Guid createdDebtId = await debtsService.CreateDebt(createDebtRequest, email);

            return Ok(new Response<object>
            {
                Message = null,
                Payload = createdDebtId,
                StatusCode = HttpStatusCode.Created
            });
        }

        [HttpDelete]
        [Route("delete-debt")]
        public async Task<ActionResult<object>> DeleteDebt([FromQuery] string id, [FromQuery] string email)
        {
            if (!IsRequestFromValidUser(email))
            {
                throw new ForbiddenRequestException();
            }

            await debtsService.DeleteDebt(id, email);

            return Ok(new Response<object>
            {
                Message = null,
                Payload = null,
                StatusCode = HttpStatusCode.OK
            });
        }

        [HttpPut]
        [Route("edit-debt")]
        public async Task<ActionResult<object>> EditDebt([FromBody] EditDebtRequest editDebtRequest, [FromQuery(Name = "email")] string email)
        {
            if (!IsRequestFromValidUser(email))
            {
                throw new ForbiddenRequestException();
            }

            await debtsService.EditDebt(editDebtRequest, email);

            return Ok(new Response<object>
            {
                Message = null,
                Payload = null,
                StatusCode = HttpStatusCode.OK
            });
        }

        [HttpPut]
        [Route("approve-debt")]
        [Authorize]
        public async Task<ActionResult<object>> ApproveDebt([FromQuery] string debtId, [FromQuery(Name = "email")] string email)
        {
            if (!IsRequestFromValidUser(email))
            {
                throw new ForbiddenRequestException();
            }

            await debtsService.ApproveDebt(debtId, email);

            return Ok(new Response<object>
            {
                Message = null,
                Payload = null,
                StatusCode = HttpStatusCode.OK
            });
        }

        [HttpPut]
        [Route("reject-debt")]
        [Authorize]
        public async Task<ActionResult<object>> RejectDebt([FromQuery] string debtId, [FromQuery(Name = "email")] string email)
        {
            if (!IsRequestFromValidUser(email))
            {
                throw new ForbiddenRequestException();
            }

            await debtsService.RejectDebt(debtId, email);

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
