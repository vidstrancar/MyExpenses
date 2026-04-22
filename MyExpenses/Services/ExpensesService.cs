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
        try
        {
            var newExpense = createExpenseRequest.ToExpense();
            context.Expenses.Add(newExpense);
            await context.SaveChangesAsync();
            return newExpense;
        }
        catch (DbUpdateException exception)
        {
            throw new Exception("Could not save new expense to the database", exception);
        }
        catch (Exception exception)
        {
            throw new Exception("Unexpected exception in CreateExpenseAsync", exception);
        }
    }

    public async Task<Expense?> UpdateExpenseAsync(int id, UpdateExpenseRequest updateExpenseRequest)
    {
        try
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
        catch (DbUpdateException exception)
        {
            throw new Exception("Could not update expense in the database", exception);
        }
        catch (Exception exception)
        {
            throw new Exception("Unexpected exception in UpdateExpenseAsync", exception);
        }
        
    }

    public async Task<bool> DeleteExpenseAsync(int id)
    {
        try
        {
            var existingExpense = await context.Expenses.FindAsync(id);
            if (existingExpense == null) return false;
        
            context.Expenses.Remove(existingExpense);
            await context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException exception)
        {
            throw new Exception("Could not delete expense from the database", exception);
        }
        catch (Exception exception)
        {
            throw new Exception("Unexpected exception in DeleteExpenseAsync", exception);
        }       
    }
}