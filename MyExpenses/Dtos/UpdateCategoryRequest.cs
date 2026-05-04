using System.ComponentModel.DataAnnotations;

namespace MyExpenses.Dtos;

public class UpdateCategoryRequest
{
    [StringLength(100)]
    public string? Name { get; set; }
    
    public int? ParentId { get; set; }
}