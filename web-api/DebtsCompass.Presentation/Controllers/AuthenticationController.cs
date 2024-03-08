using DebtsCompass.Domain;
using DebtsCompass.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Entities.Models;
using DebtsCompass.Domain.Entities.Requests;
using Microsoft.AspNetCore.WebUtilities;
using DebtsCompass.Domain.Entities.Dtos;
using EmailSender;

namespace DebtsCompass.Presentation.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly IJwtService jwtService;
        private readonly IAuthenticationService authenticationService;
        private readonly IEmailService emailService;
        public AuthenticationController(IJwtService jwtService, IAuthenticationService authenticationService, UserManager<User> userManager, IEmailService emailService)
        {
            this.jwtService = jwtService;
            this.authenticationService = authenticationService;
            this.userManager = userManager;
            this.emailService = emailService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<Response<LoginResponse>>> Login([FromBody] LoginRequest loginRequest)
        {
            LoginResponse loginResponse = await authenticationService.GetLoginResponse(loginRequest);

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
            User user = await authenticationService.Register(registerRequest);

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var param = new Dictionary<string, string?>
                        {
                            {"token", token },
                            {"email", user.Email }
                        };

            var callback = QueryHelpers.AddQueryString(registerRequest.ClientURI, param);
            ReceiverInfoDto receiverInfoDto = Mapper.UserToReceiverInfoDto(user);
            await emailService.SendEmailConfirmationNotification(receiverInfoDto, callback);

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

            var refreshTokenResponse = await jwtService.GetRefreshToken(refreshTokenRequest.Email, refreshTokenRequest);
            await jwtService.UpdateRefreshToken(refreshTokenRequest.Email, refreshTokenResponse.RefreshToken);

            return Ok(new Response<RefreshTokenResponse>
            {
                Message = null,
                Payload = refreshTokenResponse,
                StatusCode = HttpStatusCode.OK  
            });
        }

        [HttpGet("email-confirmation")]
        public async Task<ActionResult<Response<object>>> EmailConfirmation([FromQuery] string email, [FromQuery] string token)
        {
            await authenticationService.ConfirmEmail(email, token);

            return Ok(new Response<object>
            {
                Message = null,
                Payload = null,
                StatusCode = HttpStatusCode.OK
            });
        }


    }
}
