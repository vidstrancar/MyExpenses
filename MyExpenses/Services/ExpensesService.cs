using Microsoft.EntityFrameworkCore;
using MyExpenses.Data;
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

    public Task<Expense> AddExpenseAsync(Expense expense)
    {
        throw new NotImplementedException();
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