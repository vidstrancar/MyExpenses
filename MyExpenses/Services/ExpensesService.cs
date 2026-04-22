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
        var newExpense = createExpenseRequest.ToExpense();
        context.Expenses.Add(newExpense); 
        await context.SaveChangesAsync();
        
        return newExpense;
    }

    public async Task<Expense?> UpdateExpenseAsync(int id, UpdateExpenseRequest updateExpenseRequest)
    {
        var existingExpense = await context.Expenses.FindAsync(id);
        if (existingExpense == null) return null;
        
        existingExpense.Category = updateExpenseRequest.Category;
        existingExpense.Description = updateExpenseRequest.Description;
        existingExpense.Amount = updateExpenseRequest.Amount;
        existingExpense.Date = updateExpenseRequest.Date;
        
        await context.SaveChangesAsync();
        return existingExpense; 
    }

    public async Task<bool> DeleteExpenseAsync(int id)
    {
        var existingExpense = await context.Expenses.FindAsync(id);
        if (existingExpense == null) return false;
        
        context.Expenses.Remove(existingExpense);
        await context.SaveChangesAsync();
        return true;
    }
}