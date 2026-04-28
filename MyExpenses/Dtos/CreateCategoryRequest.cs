using System.ComponentModel.DataAnnotations;
using MyExpenses.Models;

namespace MyExpenses.Dtos;

public class CreateCategoryRequest
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(100)]
    public string? Parent { get; set; } = string.Empty;
}