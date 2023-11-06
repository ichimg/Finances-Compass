using DebtsCompass.Application.Exceptions;
using DebtsCompass.Application.Services;
using DebtsCompass.Domain;
using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<List<UserDto>>> GetFriends([FromHeader] string email)
        {
            var userIdentity = User.Identity as ClaimsIdentity;
            var userEmailClaim = userIdentity.FindFirst(ClaimTypes.Email)?.Value;

            if (!string.Equals(userEmailClaim, email, StringComparison.OrdinalIgnoreCase))
            {
                throw new ForbiddenRequestException();
            }

            var friends = await friendshipsService.GetUserFriendsByEmail(email);

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
