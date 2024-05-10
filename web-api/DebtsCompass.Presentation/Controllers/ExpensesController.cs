using DebtsCompass.Domain.Entities.Requests;
using DebtsCompass.Domain;
using DebtsCompass.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using DebtsCompass.Application.Exceptions;
using System.Security.Claims;
using DebtsCompass.Application.Services;
using DebtsCompass.Domain.Entities.DtoResponses;
using DebtsCompass.Domain.Entities.Models;
using Newtonsoft.Json;

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

            return Ok(new Response<Guid>
            {
                Message = null,
                Payload = createdExpenseId,
                StatusCode = HttpStatusCode.Created
            });
        }

        [HttpDelete]
        [Route("delete-expense")]
        [Authorize]
        public async Task<ActionResult<object>> DeleteDebt([FromQuery] string id, [FromQuery] string email)
        {
            if (!IsRequestFromValidUser(email))
            {
                throw new ForbiddenRequestException();
            }

            await expensesService.DeleteExpense(id, email);

            return Ok(new Response<object>
            {
                Message = null,
                Payload = null,
                StatusCode = HttpStatusCode.OK
            });
        }

        [HttpPut]
        [Route("edit-expense")]
        [Authorize]
        public async Task<ActionResult<object>> EditExpense([FromBody] EditExpenseRequest editExpenseRequest, [FromQuery(Name = "email")] string email)
        {
            if (!IsRequestFromValidUser(email))
            {
                throw new ForbiddenRequestException();
            }

            await expensesService.EditExpense(editExpenseRequest, email);

            return Ok(new Response<object>
            {
                Message = null,
                Payload = null,
                StatusCode = HttpStatusCode.OK
            });
        }

        [HttpGet]
        [Route("get-expenses-incomes")]
        [Authorize]
        public async Task<ActionResult<List<ExpenseOrIncomeDto>>> GetAllExpensesAndIncomes([FromHeader] string email,
            [FromQuery] YearMonthDto yearMonthDto)
        {
            if (!IsRequestFromValidUser(email))
            {
                throw new ForbiddenRequestException();
            }

            var expensesAndIncomes = await expensesService.GetAllByEmail(email, yearMonthDto);

            var metadata = new
            {
                expensesAndIncomes.TotalAmountExpenses,
                expensesAndIncomes.TotalAmountIncomes,
            };

            Response.Headers.Add("X-Total", JsonConvert.SerializeObject(metadata));

            Response<List<ExpenseOrIncomeDto>> response = new Response<List<ExpenseOrIncomeDto>>
            {
                Message = null,
                Payload = expensesAndIncomes,
                StatusCode = HttpStatusCode.OK
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("get-expenses-incomes-count")]
        [Authorize]
        public async Task<ActionResult<TotalExpensesAndIncomesDto>> GetExpensesAndIncomesTotalCount([FromHeader] string email)
        {
            if (!IsRequestFromValidUser(email))
            {
                throw new ForbiddenRequestException();
            }

            TotalExpensesAndIncomesDto expensesAndIncomes = await expensesService.GetExpensesAndIncomesTotalCount(email);

            Response<TotalExpensesAndIncomesDto> response = new Response<TotalExpensesAndIncomesDto>
            {
                Message = null,
                Payload = expensesAndIncomes,
                StatusCode = HttpStatusCode.OK
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("get-annual-expenses-by-category")]
        [Authorize]
        public async Task<ActionResult<Response<IEnumerable<ExpenseBarChartDto>>>> GetAnnualExpensesByCategory([FromHeader] string email)
        {
            if (!IsRequestFromValidUser(email))
            {
                throw new ForbiddenRequestException();
            }

            IEnumerable<ExpenseBarChartDto> expenseData = await expensesService.GetAnnualExpensesByCategory(email);

            Response<IEnumerable<ExpenseBarChartDto>> response = new Response<IEnumerable<ExpenseBarChartDto>>
            {
                Message = null,
                Payload = expenseData,
                StatusCode = HttpStatusCode.OK
            };

            return Ok(response);
        }

        private bool IsRequestFromValidUser(string email)
        {
            var userIdentity = User.Identity as ClaimsIdentity;
            var userEmailClaim = userIdentity.FindFirst(ClaimTypes.Email)?.Value;

            return string.Equals(userEmailClaim, email, StringComparison.OrdinalIgnoreCase);
        }
    }
}
