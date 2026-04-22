using MyExpenses.Models;

namespace MyExpenses.Dtos;

public class UpdateExpenseRequest
{
    public int Id { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}