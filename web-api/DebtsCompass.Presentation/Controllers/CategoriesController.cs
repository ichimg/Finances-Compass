using DebtsCompass.Application.Exceptions;
using DebtsCompass.Application.Services;
using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using DebtsCompass.Domain.Interfaces;
using DebtsCompass.Domain.Entities.Dtos;

namespace DebtsCompass.Presentation.Controllers
{
    [ApiController]
    [Route("api")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesService categoriesService;

        public CategoriesController( ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        [HttpGet]
        [Route("get-categories")]
        [Authorize]
        public async Task<ActionResult<List<ExpenseCategoryDto>>> GetCategories([FromHeader] string email)
        {
            var categories = await categoriesService.GetAllByEmail(email);

            Response<List<ExpenseCategoryDto>> response = new Response<List<ExpenseCategoryDto>>
            {
                Message = null,
                Payload = categories,
                StatusCode = HttpStatusCode.OK
            };

            return Ok(response);
        }
    }
}
