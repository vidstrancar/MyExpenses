using MyExpenses.Models;

namespace MyExpenses.Services;

public interface IExpensesService
{
    public Task<List<Expense>> GetAllExpensesAsync();
    public Task<Expense?> GetExpenseByIdAsync(int id);
    public Task<Expense> AddExpenseAsync(Expense expense);
    public Task<bool> UpdateExpenseAsync(int id, Expense expense);
    public Task<bool> DeleteExpenseAsync(int id);
}