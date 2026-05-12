using Microsoft.EntityFrameworkCore;
using MyExpenses.Data;
using MyExpenses.Dtos;
using MyExpenses.Exceptions;
using MyExpenses.Models;

namespace MyExpenses.Services;

public class ExpensesService(
    AppDbContext context,
    IUserContext userContext,
    ILogger<ExpensesService> logger): IExpensesService
{
    
    private Guid CurrentUserId => userContext.UserId;
    
    public async Task<List<Expense>> GetAllExpensesAsync()
    {
        var result = await context.Expenses
            .Where(expense => expense.UserId == CurrentUserId)
            .Include(e => e.Category)
            .ToListAsync();
        return result;
    }

    public async Task<Expense?> GetExpenseByIdAsync(int id)
    {
        var result = await context.Expenses
            .Where(expense => expense.UserId == CurrentUserId)
            .Include(e => e.Category)
            .FirstOrDefaultAsync(e => e.Id == id);
        return result;
    }

    public async Task<Expense> CreateExpenseAsync(CreateExpenseRequest createExpenseRequest)
    {
        var categoryId = createExpenseRequest.CategoryId;
        if (categoryId is not null)
        {
            var categoryExists = await context.Categories
                .AnyAsync(cat => cat.Id == categoryId && cat.UserId == userContext.UserId);
            
            if (!categoryExists)
            {
                throw new ExpensesServiceException(
                    $"Category with id {categoryId} does not exist",
                    StatusCodes.Status400BadRequest,
                    null!);
            }
        }
        
        var newExpense = createExpenseRequest.ToExpense();
        newExpense.UserId = CurrentUserId;
        context.Expenses.Add(newExpense);
        
        try
        {
            await context.SaveChangesAsync();
            return await context.Expenses
                .Where(e => e.UserId == CurrentUserId)
                .Include(e => e.Category)
                .FirstAsync(e => e.Id == newExpense.Id);
        }
        catch (DbUpdateException exception)
        {
            logger.LogError(exception, "Database error creating new expense");
            throw new ExpensesServiceException(
                "Could not save new expense to the database",
                StatusCodes.Status400BadRequest,
                exception);
        }
    }

    public async Task<Expense?> UpdateExpenseAsync(int id, UpdateExpenseRequest updateExpenseRequest)
    {
        var existingExpense =
            await context
                .Expenses
                .Include(expense => expense.Category)
                .FirstOrDefaultAsync(expense => expense.Id == id && expense.UserId == CurrentUserId);
        
        if (existingExpense == null) return null;
        
        var categoryId = updateExpenseRequest.CategoryId;
        if (categoryId is not null)
        {
            var categoryExists = await context.Categories
                .AnyAsync(cat => cat.Id == categoryId && cat.UserId == CurrentUserId);
            
            if (!categoryExists)
            {
                throw new ExpensesServiceException(
                    $"Category with id {categoryId} does not exist",
                    StatusCodes.Status400BadRequest,
                    null!);
            }
        }
        
        existingExpense.CategoryId = updateExpenseRequest.CategoryId;
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
            throw new ExpensesServiceException(
                "The expense was already modified or deleted by another user.",
                StatusCodes.Status409Conflict,
                exception);
        }
        catch (DbUpdateException exception)
        {
            logger.LogError(exception, "Database error updating expense {Id}", id);
            throw new ExpensesServiceException("Could not update expense in the database", exception);
        }
    }

    public async Task<bool> DeleteExpenseAsync(int id)
    {
        var existingExpense = await context.Expenses
            .Where(e => e.UserId == CurrentUserId)
            .FirstOrDefaultAsync(e => e.Id == id);
        
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
            throw new ExpensesServiceException(
                "The expense was already modified or deleted by another user.",
                StatusCodes.Status409Conflict,
                exception);
        }
        catch (DbUpdateException exception)
        {
            logger.LogError(exception, "Database error deleting expense {Id}", id);
            throw new ExpensesServiceException("Could not delete expense from the database", exception);
        }
    }
}