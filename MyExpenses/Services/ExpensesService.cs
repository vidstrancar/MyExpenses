using Microsoft.EntityFrameworkCore;
using MyExpenses.Data;
using MyExpenses.Dtos;
using MyExpenses.Models;

namespace MyExpenses.Services;

public class ExpensesService(AppDbContext context): IExpensesService
{
    public async Task<List<Expense>> GetAllExpensesAsync()
    {
        var result = await context.Expenses.ToListAsync();
        return result;
    }

    public async Task<Expense?> GetExpenseByIdAsync(int id)
    {
        var result = await context.Expenses.FindAsync(id);
        return result;
    }

    public async Task<Expense> CreateExpenseAsync(CreateExpenseRequest createExpenseRequest)
    {
        var newExpense = new Expense()
        {
            Category = createExpenseRequest.Category,
            Description = createExpenseRequest.Description,
            Amount = createExpenseRequest.Amount,
            Date = createExpenseRequest.Date
        };
        
        context.Expenses.Add(newExpense); 
        await context.SaveChangesAsync();
        
        return newExpense;
    }

    public Task<bool> UpdateExpenseAsync(int id, Expense expense)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteExpenseAsync(int id)
    {
        throw new NotImplementedException();
    }
}