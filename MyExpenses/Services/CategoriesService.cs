using Microsoft.EntityFrameworkCore;
using MyExpenses.Data;
using MyExpenses.Dtos;
using MyExpenses.Exceptions;
using MyExpenses.Models;

namespace MyExpenses.Services;

public class CategoriesService(AppDbContext context, ILogger<CategoriesService> logger): ICategoriesService
{
    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        var result = await context.Categories.ToListAsync();
        return result;
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        var result = await context.Categories.FindAsync(id);
        return result;
    }

    public async Task<Category> CreateCategoryAsync(CreateCategoryRequest createCategoryRequest)
    {
        var category = createCategoryRequest.ToCategory();
        context.Categories.Add(category);

        try
        {
            await context.SaveChangesAsync();
            return category;
        }
        catch (DbUpdateException exception)
        {
            logger.LogError(exception, "Database error creating new category");
            throw new CategoriesServiceException(
                "Could not save new category to the database",
                StatusCodes.Status400BadRequest,
                exception);
        }
    }

    public async Task<Category?> UpdateCategoryAsync(int id, UpdateCategoryRequest updateCategoryRequest)
    {
        var category = await context.Categories.FindAsync(id);
        if (category == null) return null;

        var newName = updateCategoryRequest.Name;
        if (!string.IsNullOrEmpty(newName)) category.Name = newName;
        if (updateCategoryRequest.ParentId != null) category.ParentId = updateCategoryRequest.ParentId;

        try
        {
            await context.SaveChangesAsync();
            return category;
        }
        catch (DbUpdateConcurrencyException exception)
        {
            logger.LogError(exception, "Concurrency conflict updating category {Id}", id);
            throw new CategoriesServiceException(
                $"The category was already modified or deleted by another user.",
                StatusCodes.Status409Conflict,
                exception);
        }
        catch (DbUpdateException exception)
        {
            logger.LogError(exception, "Database error updating category {Id}", id);
            throw new CategoriesServiceException("Could not update category in the database", exception);
        }
    }
    
    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await context.Categories.FindAsync(id);
        if (category == null) return false;

        context.Categories.Remove(category);

        try
        {
            await context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException exception)
        {
            logger.LogError(exception, "Concurrency conflict deleting category {Id}", id);
            throw new CategoriesServiceException(
                $"The category was already modified or deleted by another user.",
                StatusCodes.Status409Conflict,
                exception);
        }
        catch (DbUpdateException exception)
        {
            logger.LogError(exception, "Database error deleting category {Id}", id);
            throw new CategoriesServiceException("Could not delete category from the database", exception);
        }
    }
}