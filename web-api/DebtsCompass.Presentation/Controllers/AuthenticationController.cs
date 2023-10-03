using DebtsCompass.Application.Exceptions;
using DebtsCompass.Domain;
using DebtsCompass.Domain.Interfaces;
using DebtsCompass.Domain.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DebtsCompass.Presentation.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IJwtService jwtService;
        private readonly IAuthenticationService authenticationService;

        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
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

            LoginResponse loginResponse = new LoginResponse
            {
                Email = loginRequest.Email,
                Token = jwtService.GenerateToken(loginRequest.Email)
            };

            return Ok(new Response<LoginResponse>
            {
                Message = null,
                Payload = loginResponse,
                StatusCode = HttpStatusCode.OK
            });
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }


}
