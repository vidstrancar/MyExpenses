using MyExpenses.Models;

namespace MyExpenses.Dtos;

public class CreateExpenseRequest
{
    public string? Category { get; set; }
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }

    public Expense ToExpense()
    {
        return new Expense()
        {
            Category = Category,
            Description = Description,
            Amount = Amount,
            Date = Date
        };
    }  
}