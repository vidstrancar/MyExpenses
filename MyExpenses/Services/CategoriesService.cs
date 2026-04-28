using Microsoft.EntityFrameworkCore;
using MyExpenses.Data;
using MyExpenses.Dtos;
using MyExpenses.Models;

namespace MyExpenses.Services;

public class CategoriesService(AppDbContext context): ICategoriesService
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

    public Task<Category> CreateCategoryAsync(CreateCategoryRequest createCategoryRequest)
    {
        throw new NotImplementedException();
    }

    public Task<Category?> UpdateCategoryAsync(UpdateCategoryRequest createCategoryRequest)
    {
        throw new NotImplementedException();
    }
    
    public Task<bool> DeleteCategoryAsync(int id)
    {
        throw new NotImplementedException();
    }
}