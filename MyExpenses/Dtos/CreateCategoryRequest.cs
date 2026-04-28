using System.ComponentModel.DataAnnotations;
using MyExpenses.Models;

namespace MyExpenses.Dtos;

public class CreateCategoryRequest
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    public int? ParentId { get; set; }

    public Category ToCategory()
    {
        return new Category()
        {
            Name = Name,
            ParentId = ParentId
        };
    }
}