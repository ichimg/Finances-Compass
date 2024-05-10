using DebtsCompass.Application.Exceptions;
using DebtsCompass.Domain;
using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Interfaces;
using DebtsCompass.Domain.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;

namespace DebtsCompass.Presentation.Controllers
{
    [ApiController]
    [Route("api")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService usersService;
        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [HttpGet]
        [Route("search-users")]
        [Authorize]
        public async Task<ActionResult<PagedList<UserDto>>> SearchUsers(
            [FromHeader] string email,
            [FromQuery] string searchQuery,
            [FromQuery] PagedParameters pagedParameters)
        {
            var userIdentity = User.Identity as ClaimsIdentity;
            var userEmailClaim = userIdentity.FindFirst(ClaimTypes.Email)?.Value;

            if (!string.Equals(userEmailClaim, email, StringComparison.OrdinalIgnoreCase))
            {
                throw new ForbiddenRequestException();
            }

            var users = await usersService.SearchUsers(searchQuery, email, pagedParameters); 

            var metadata = new
            {
                users.TotalCount,
                users.PageSize,
                users.CurrentPage,
                users.TotalPages,
                users.HasNext,
                users.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(new Response<List<UserDto>>
            {
                Message = null,
                Payload = users,
                StatusCode = HttpStatusCode.OK
            });
        }


        [HttpGet("get-dashboard-year")]
        [Authorize]
        public async Task<ActionResult<Response<YearsDto>>> GetDashboardYear([FromQuery] string email)
        {
            if (!IsRequestFromValidUser(email))
            {
                throw new ForbiddenRequestException();
            }

            YearsDto yearsDto = await usersService.GetDashboardYear(email);

            return Ok(new Response<YearsDto>
            {
                Message = null,
                Payload = yearsDto,
                StatusCode = HttpStatusCode.OK
            });
        }

        [HttpPut("change-dashboard-year")]
        [Authorize]
        public async Task<ActionResult<Response<object>>> ChangeDashboardYear([FromQuery] string email, [FromQuery] int year)
        {
            if (!IsRequestFromValidUser(email))
            {
                throw new ForbiddenRequestException();
            }

            await usersService.ChangeDashboardYear(email, year);

            return Ok(new Response<object>
            {
                Message = null,
                Payload = null,
                StatusCode = HttpStatusCode.OK
            });
        }

        [HttpGet("get-currency-preference")]
        [Authorize]
        public async Task<ActionResult<Response<string>>> GetCurrencyPreference([FromQuery] string email)
        {
            string currency = await usersService.GetUserCurrencyPreference(email);

            return Ok(new Response<string>
            {
                Message = null,
                Payload = currency,
                StatusCode = HttpStatusCode.OK
            });
        }

        [HttpPut("change-currency-preference")]
        [Authorize]
        public async Task<ActionResult<Response<object>>> ChangeCurrencyPreference([FromQuery] string email, [FromQuery] string currency)
        {
            if (!IsRequestFromValidUser(email))
            {
                throw new ForbiddenRequestException();
            }

            await usersService.ChangeUserCurrencyPreference(email, currency);

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
