using MyExpenses.Dtos;
using MyExpenses.Models;

namespace MyExpenses.Services;

public interface IExpensesService
{
    public Task<List<Expense>> GetAllExpensesAsync();
    public Task<Expense?> GetExpenseByIdAsync(int id);
    public Task<Expense> CreateExpenseAsync(CreateExpenseRequest createExpenseRequest);
    public Task<Expense?> UpdateExpenseAsync(int id, UpdateExpenseRequest updateExpenseRequest);
    public Task<bool> DeleteExpenseAsync(int id);
}