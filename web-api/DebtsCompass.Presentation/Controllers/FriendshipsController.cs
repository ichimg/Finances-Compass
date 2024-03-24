using DebtsCompass.Application.Exceptions;
using DebtsCompass.Domain;
using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Entities.Requests;
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
            if (!IsRequestFromValidUser(email))
            {
                throw new ForbiddenRequestException();
            }

            var friends = await friendshipsService.GetUserFriendsById(email, pagedParameters);

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

        [HttpGet]
        [Route("friend-requests")]
        [Authorize]
        public async Task<ActionResult<PagedList<UserDto>>> GetFriendRequests([FromHeader] string email, [FromQuery] PagedParameters pagedParameters)
        {
            if (!IsRequestFromValidUser(email))
            {
                throw new ForbiddenRequestException();
            }

            var friends = await friendshipsService.GetUserFriendRequestsById(email, pagedParameters);

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

        [HttpPost]
        [Route("add-friend")]
        [Authorize]
        public async Task<ActionResult<object>> AddFriend([FromBody] FriendRequest friendRequest)
        {
            if (!IsRequestFromValidUser(friendRequest.RequesterUserEmail))
            {
                throw new ForbiddenRequestException();
            }

            await friendshipsService.AddFriend(friendRequest);

            return Ok(new Response<object>
            {
                Message = null,
                Payload = null,
                StatusCode = HttpStatusCode.OK
            });
        }

        [HttpDelete]
        [Route("cancel-friend")]
        [Authorize]
        public async Task<ActionResult<object>> CancelFriend([FromBody] FriendRequestDto deleteFriendRequest)
        {
            if (!IsRequestFromValidUser(deleteFriendRequest.RequesterUserEmail))
            {
                throw new ForbiddenRequestException();
            }

            await friendshipsService.DeleteFriendRequest(deleteFriendRequest);

            return Ok(new Response<object>
            {
                Message = null,
                Payload = null,
                StatusCode = HttpStatusCode.OK
            });
        }


        [HttpPut]
        [Route("accept-friend-request")]
        [Authorize]
        public async Task<ActionResult<object>> AcceptFriendRequest([FromBody] FriendRequestDto friendRequestDto)
        {
            if (!IsRequestFromValidUser(friendRequestDto.RequesterUserEmail))
            {
                throw new ForbiddenRequestException();
            }

            await friendshipsService.AcceptFriendRequest(friendRequestDto);

            return Ok(new Response<object>
            {
                Message = null,
                Payload = null,
                StatusCode = HttpStatusCode.OK
            });
        }

        [HttpPut]
        [Route("reject-friend-request")]
        [Authorize]
        public async Task<ActionResult<object>> RejectFriendRequest([FromBody] FriendRequestDto friendRequestDto)
        {
            if (!IsRequestFromValidUser(friendRequestDto.RequesterUserEmail))
            {
                throw new ForbiddenRequestException();
            }

            await friendshipsService.RejectFriendRequest(friendRequestDto);

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
