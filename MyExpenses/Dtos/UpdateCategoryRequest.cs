using System.ComponentModel.DataAnnotations;

namespace MyExpenses.Dtos;

public class UpdateCategoryRequest
{
    [Required]
    public int Id { get; set; }
    
    [StringLength(100)]
    public string? Name { get; set; } = string.Empty;
    
    [StringLength(100)]
    public string? Parent { get; set; } = string.Empty;
}