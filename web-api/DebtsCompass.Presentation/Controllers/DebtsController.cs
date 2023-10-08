using DebtsCompass.Application.Exceptions;
using DebtsCompass.Domain;
using DebtsCompass.Domain.DtoResponses;
using DebtsCompass.Domain.Interfaces;
using DebtsCompass.Domain.Services;
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
        [Route("view-debts")]
        [Authorize]
        public async Task<ActionResult<List<DebtDto>>> GetDebts([FromHeader] string email)
        {
            var userIdentity = User.Identity as ClaimsIdentity;
            var userEmailClaim = userIdentity.FindFirst(ClaimTypes.Email)?.Value;

            if (!string.Equals(userEmailClaim, email, StringComparison.OrdinalIgnoreCase))
            {
                throw new ForbiddenRequestException();
            }

            var debts = await debtsService.GetAll(email);

            Response<List<DebtDto>> response = new Response<List<DebtDto>>
            {
                Message = null,
                Payload = debts,
                StatusCode = HttpStatusCode.OK
            };

            return Ok(response);
        }
    }
}
