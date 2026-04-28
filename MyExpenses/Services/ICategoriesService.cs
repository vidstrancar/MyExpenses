using MyExpenses.Dtos;
using MyExpenses.Models;

namespace MyExpenses.Services;

public interface ICategoriesService
{
    public Task<List<Category>> GetAllCategoriesAsync();
    public Task<Category?> GetCategoryByIdAsync(int id);
    public Task<Category> CreateCategoryAsync(CreateCategoryRequest createCategoryRequest);
    public Task<Category?> UpdateCategoryAsync(UpdateCategoryRequest createCategoryRequest);
    public Task<bool> DeleteCategoryAsync(int id);
}