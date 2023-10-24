using DebtsCompass.Application.Exceptions;
using DebtsCompass.Domain;
using DebtsCompass.Domain.DtoResponses;
using DebtsCompass.Domain.Interfaces;
using DebtsCompass.Domain.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace DebtsCompass.Presentation.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IJwtService jwtService;
        private readonly IAuthenticationService authenticationService;
        public AuthenticationController(IJwtService jwtService, IAuthenticationService authenticationService)
        {
            this.jwtService = jwtService;
            this.authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<Response<LoginResponse>>> Login([FromBody] LoginRequest loginRequest)
        {
            if (!await authenticationService.IsValidLogin(loginRequest))
            {
                throw new InvalidCredentialsException();
            }

            string refreshToken = jwtService.GenerateRefreshToken();
            await jwtService.UpdateRefreshToken(loginRequest.Email, refreshToken);

            LoginResponse loginResponse = new LoginResponse
            {
                Email = loginRequest.Email,
                AccessToken = jwtService.GenerateToken(loginRequest.Email),
                RefreshToken = refreshToken
            };


            return Ok(new Response<LoginResponse>
            {
                Message = null,
                Payload = loginResponse,
                StatusCode = HttpStatusCode.OK
            });
        }

        [HttpPost("register")]
        public async Task<ActionResult<Response<object>>> Register([FromBody] RegisterRequest registerRequest)
        {
            await authenticationService.Register(registerRequest);

            return Ok(new Response<object>
            {
                Message = null,
                Payload = null,
                StatusCode = HttpStatusCode.OK
            });
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<ActionResult<Response<RefreshTokenResponse>>> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            if (refreshTokenRequest is null)
            {
                return BadRequest("Invalid request");
            }

            var userIdentity = User.Identity as ClaimsIdentity;
            var userEmailClaim = userIdentity.FindFirst(ClaimTypes.Email)?.Value;

            var refreshTokenResponse = await jwtService.GetRefreshToken(userEmailClaim, refreshTokenRequest);
            await jwtService.UpdateRefreshToken(userEmailClaim, refreshTokenResponse.RefreshToken);

            return Ok(new Response<RefreshTokenResponse>
            {
                Message = null,
                Payload = refreshTokenResponse,
                StatusCode = HttpStatusCode.OK
            });
        }


    }
}
