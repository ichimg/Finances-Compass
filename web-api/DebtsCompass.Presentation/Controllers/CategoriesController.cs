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
        [Route("get-expense-categories")]
        [Authorize]
        public async Task<ActionResult<List<CategoryDto>>> GetExpenseCategories([FromHeader] string email)
        {
            var categories = await categoriesService.GetAllExpenseCategoriesByEmail(email);

            Response<List<CategoryDto>> response = new Response<List<CategoryDto>>
            {
                Message = null,
                Payload = categories,
                StatusCode = HttpStatusCode.OK
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("get-income-categories")]
        [Authorize]
        public async Task<ActionResult<List<CategoryDto>>> GetIncomeCategories([FromHeader] string email)
        {
            var categories = await categoriesService.GetAllIncomeCategoriesByEmail(email);

            Response<List<CategoryDto>> response = new Response<List<CategoryDto>>
            {
                Message = null,
                Payload = categories,
                StatusCode = HttpStatusCode.OK
            };

            return Ok(response);
        }
    }
}
