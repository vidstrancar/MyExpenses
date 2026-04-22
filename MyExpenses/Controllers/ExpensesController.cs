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
        public async Task<ActionResult<Expense>> AddExpense(CreateExpenseRequest createExpenseRequest)
        {
            var createdExpense = await expensesService.CreateExpenseAsync(createExpenseRequest);
            return CreatedAtAction(nameof(GetExpenseById), new { id = createdExpense.Id }, createdExpense);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Expense>> UpdateExpense(UpdateExpenseRequest updateExpenseRequest)
        {
            var updatedExpense = await expensesService.UpdateExpenseAsync(updateExpenseRequest.Id, updateExpenseRequest);

            if (updatedExpense == null)
            {
                return NotFound($"Expense with id {updateExpenseRequest.Id} not found");   
            }
            
            return Ok(updatedExpense);
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteExpense(int id)
        {
            var deleted = await expensesService.DeleteExpenseAsync(id);
            return deleted ? Ok($"Expense with id {id} is deleted") : NotFound($"Expense with id {id} not found");
        }       
    }
}
