using Microsoft.EntityFrameworkCore;
using MyExpenses.Data;
using MyExpenses.Dtos;
using MyExpenses.Exceptions;
using MyExpenses.Models;

namespace MyExpenses.Services;

public class ExpensesService(AppDbContext context, ILogger<ExpensesService> logger): IExpensesService
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
        
        try
        {
            await context.SaveChangesAsync();
            return newExpense;
        }
        catch (DbUpdateException exception)
        {
            logger.LogError(exception, "Database error creating new expense");
            throw new ExpensesServiceException("Could not save new expense to the database", exception);
        }
    }

    public async Task<Expense?> UpdateExpenseAsync(int id, UpdateExpenseRequest updateExpenseRequest)
    {
        var existingExpense = await context.Expenses.FindAsync(id);
        if (existingExpense == null) return null;
        
        existingExpense.Category = updateExpenseRequest.Category;
        existingExpense.Description = updateExpenseRequest.Description;
        existingExpense.Amount = updateExpenseRequest.Amount;
        existingExpense.Date = updateExpenseRequest.Date;

        try
        {
            await context.SaveChangesAsync();
            return existingExpense;
        }
        catch (DbUpdateConcurrencyException exception)
        {
            logger.LogError(exception, "Concurrency conflict updating expense {Id}", id);
            throw new ExpensesServiceException("The expense was already modified or deleted by another user.", exception);
        }
        catch (DbUpdateException exception)
        {
            logger.LogError(exception, "Database error updating expense {Id}", id);
            throw new ExpensesServiceException("Could not update expense in the database", exception);
        }
    }

    public async Task<bool> DeleteExpenseAsync(int id)
    {
        var existingExpense = await context.Expenses.FindAsync(id);
        if (existingExpense == null) return false;
        
        context.Expenses.Remove(existingExpense);
        
        try
        {
            await context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException exception)
        {
            logger.LogError(exception, "Concurrency conflict deleting expense {Id}", id);
            throw new ExpensesServiceException("The expense was already modified or deleted by another user.", exception);
        }
        catch (DbUpdateException exception)
        {
            logger.LogError(exception, "Database error deleting expense {Id}", id);
            throw new ExpensesServiceException("Could not delete expense from the database", exception);
        }
    }
}