using Microsoft.AspNetCore.Http.HttpResults;
using MyExpenses.Models;

namespace MyExpenses.Services;

public class ExpensesService: IExpensesService
{
    private static readonly List<Expense> Expenses =
    [
        new Expense { Id = 1, Amount = 23.3m, Category = "Groceries", Description = "Spar", Date = DateTime.Now },
        new Expense { Id = 2, Amount = 4m, Category = "Fast food", Description = "Kebab", Date = DateTime.Now }
    ];
    
    public Task<List<Expense>> GetAllExpensesAsync()
    {
        return Task.FromResult(Expenses);
    }

    public Task<Expense?> GetExpenseByIdAsync(int id)
    {
        var result = Expenses.FirstOrDefault(e => e.Id == id);
        return Task.FromResult(result);
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