using Microsoft.EntityFrameworkCore;
using MyExpenses.Data;
using MyExpenses.Dtos;
using MyExpenses.Exceptions;
using MyExpenses.Models;

namespace MyExpenses.Services;

public class CategoriesService(
    AppDbContext context,
    IUserContext userContext,
    ILogger<CategoriesService> logger): ICategoriesService
{
    
    private Guid CurrentUserId => userContext.UserId;
    
    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        var result = await context.Categories
            .Where(c => c.UserId == CurrentUserId)
            .ToListAsync();
        return result;
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        var result = await context.Categories
            .Where(c => c.UserId == CurrentUserId)
            .FirstOrDefaultAsync(c => c.Id == id);
        return result;
    }

    public async Task<Category> CreateCategoryAsync(CreateCategoryRequest createCategoryRequest)
    {
        if (createCategoryRequest.ParentId is not null)
        {
            var parentId = createCategoryRequest.ParentId.Value;
            var parentExists = await context.Categories
                .Where(cat => cat.UserId == CurrentUserId)
                .AnyAsync(cat => cat.Id == parentId);
            
            if (!parentExists)
            {
                logger.LogError("Parent category with id {Id} does not exist", parentId);
                throw new CategoriesServiceException(
                    $"Parent category with id {parentId} does not exist",
                    StatusCodes.Status400BadRequest,
                    null!);
            }
        }
        
        var category = createCategoryRequest.ToCategory();
        context.Categories.Add(category);

        try
        {
            await context.SaveChangesAsync();
            return await context.Categories
                .Where(c => c.UserId == CurrentUserId)
                .FirstAsync(c => c.Id == category.Id);
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
        var category = await context.Categories
            .Where(c => c.UserId == CurrentUserId)
            .FirstOrDefaultAsync(c => c.Id == id);
        
        if (category == null) return null;

        var newName = updateCategoryRequest.Name;
        if (!string.IsNullOrEmpty(newName)) category.Name = newName;
        
        var newParentId = updateCategoryRequest.ParentId;
        if (newParentId is not null)
        {
            if (newParentId == id)
            {
                throw new CategoriesServiceException(
                    "Cannot set a category as its own parent",
                    StatusCodes.Status400BadRequest,
                    null!);
            }
            
            var parentExists = await context.Categories
                .Where(cat => cat.UserId == CurrentUserId)
                .AnyAsync(cat => cat.Id == newParentId);
            
            if (!parentExists)
            {
                throw new CategoriesServiceException(
                    $"Parent category with id {newParentId} does not exist",
                    StatusCodes.Status400BadRequest,
                    null!);
            }
            
            var isCircularReference = await IsCircularReference(id, newParentId); 
            if (isCircularReference)
            {
                throw new CategoriesServiceException(
                    $"Circular reference detected: the selected parent is a descendant of this category",
                    StatusCodes.Status400BadRequest,
                    null!);
            }
        }
        
        category.ParentId = updateCategoryRequest.ParentId;

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
        var category = await context.Categories
            .Where(c => CurrentUserId == c.UserId)
            .FirstOrDefaultAsync(c => c.Id == id);
        
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

    private async Task<bool> IsCircularReference(int categoryId, int? newParentId = null)
    {
        var currentId = newParentId;

        while (currentId is not null)
        {
            var parent = await context.Categories
                .AsNoTracking()
                .Where(cat => cat.Id == currentId)
                .Select(c => new { c.ParentId })
                .FirstOrDefaultAsync();

            if (parent == null) return false;
            if (parent.ParentId == categoryId) return true;
            
            currentId = parent.ParentId;
        }
        
        return false;
    }
}