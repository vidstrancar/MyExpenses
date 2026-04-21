namespace MyExpenses.Dtos;

public class CreateExpenseRequest
{
    public string? Category { get; set; }
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}