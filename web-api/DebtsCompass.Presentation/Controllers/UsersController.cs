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
    }
}
