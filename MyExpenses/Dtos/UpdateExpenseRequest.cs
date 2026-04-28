using System.ComponentModel.DataAnnotations;
using MyExpenses.Models;

namespace MyExpenses.Dtos;

public class UpdateExpenseRequest
{
    [StringLength(100)]
    public string? Category { get; set; }
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }
    
    [Required]
    public DateTime Date { get; set; }
}