using DebtsCompass.Domain.Entities.Requests;
using DebtsCompass.Domain;
using DebtsCompass.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DebtsCompass.Presentation.Controllers
{
    [ApiController]
    [Route("api")]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpensesService expensesService;
        public ExpensesController(IExpensesService expensesService)
        {
            this.expensesService = expensesService;
        }

        [HttpPost]
        [Route("create-expense")]
        [Authorize]
        public async Task<ActionResult<Guid>> CreateExpense([FromBody] CreateExpenseRequest createExpenseRequest,
          [FromQuery(Name = "email")] string email)
        {
            Guid createdExpenseId = await expensesService.CreateExpense(createExpenseRequest, email);

            return Ok(new Response<object>
            {
                Message = null,
                Payload = createdExpenseId,
                StatusCode = HttpStatusCode.Created
            });
        }

    }
}
