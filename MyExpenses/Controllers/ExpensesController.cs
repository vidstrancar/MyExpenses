using Microsoft.AspNetCore.Mvc;
using MyExpenses.Dtos;
using MyExpenses.Models;
using MyExpenses.Services;

namespace MyExpenses.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController(IExpensesService expensesService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Expense>>> GetExpenses()
        {
            return Ok(await expensesService.GetAllExpensesAsync());
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Expense?>> GetExpenseById(int id)
        {
            var expense = await expensesService.GetExpenseByIdAsync(id);
            if (expense == null)
            {
                return NotFound($"Expense with id {id} not found");
            }
            
            return Ok(expense);
        }
        
        [HttpPost]
        public async Task<ActionResult<Expense>> CreateExpense(CreateExpenseRequest? createExpenseRequest)
        {
            if (createExpenseRequest == null)
            {
                return BadRequest("Request body cannot be null");  
            }
            
            var expense = await expensesService.CreateExpenseAsync(createExpenseRequest);
            return CreatedAtAction(nameof(GetExpenseById), new { id = expense.Id }, expense);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Expense>> UpdateExpense(int id, UpdateExpenseRequest? updateExpenseRequest)
        {
            if (updateExpenseRequest == null)
            {
                return BadRequest("Request body cannot be null");
            }
            
            var expense = await expensesService.UpdateExpenseAsync(id, updateExpenseRequest);

            if (expense == null)
            {
                return NotFound($"Expense with id {id} not found");   
            }
            
            return Ok(expense);
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteExpense(int id)
        {
            var deleted = await expensesService.DeleteExpenseAsync(id);
            return deleted ? Ok($"Expense with id {id} is deleted") : NotFound($"Expense with id {id} not found");
        }       
    }
}
