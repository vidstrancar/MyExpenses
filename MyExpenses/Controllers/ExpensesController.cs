using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyExpenses.Models;

namespace MyExpenses.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private static readonly List<Expense> Expenses =
        [
            new Expense { Id = 1, Amount = 23.3m, Category = "Groceries", Description = "Spar", Date = DateTime.Now },
            new Expense { Id = 1, Amount = 4m, Category = "Fast food", Description = "Kebab", Date = DateTime.Now }
        ];

        [HttpGet]
        public async Task<ActionResult<List<Expense>>> GetExpenses() => await Task.FromResult(Ok(Expenses));
    }
}
