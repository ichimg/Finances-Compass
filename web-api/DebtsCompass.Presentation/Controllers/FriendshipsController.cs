using DebtsCompass.Application.Exceptions;
using DebtsCompass.Application.Services;
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
    public class FriendshipsController : ControllerBase
    {
        private readonly IFriendshipsService friendshipsService;

        public FriendshipsController(IFriendshipsService friendshipsService)
        {
            this.friendshipsService = friendshipsService;
        }

        [HttpGet]
        [Route("friends")]
        [Authorize]
        public async Task<ActionResult<PagedList<UserDto>>> GetFriends([FromHeader] string email, [FromQuery] PagedParameters pagedParameters)
        {
            var userIdentity = User.Identity as ClaimsIdentity;
            var userEmailClaim = userIdentity.FindFirst(ClaimTypes.Email)?.Value;

            if (!string.Equals(userEmailClaim, email, StringComparison.OrdinalIgnoreCase))
            {
                throw new ForbiddenRequestException();
            }

            var friends = await friendshipsService.GetUserFriendsByEmail(email, pagedParameters);

            var metadata = new
            {
                friends.TotalCount,
                friends.PageSize,
                friends.CurrentPage,
                friends.TotalPages,
                friends.HasNext,
                friends.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            Response<List<UserDto>> response = new Response<List<UserDto>>
            {
                Message = null,
                Payload = friends,
                StatusCode = HttpStatusCode.OK
            };

            return Ok(response);
        }    
    }
}
