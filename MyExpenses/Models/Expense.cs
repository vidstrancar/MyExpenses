using System.ComponentModel.DataAnnotations;
using MyExpenses.Entities;

namespace MyExpenses.Models;

public class Expense
{
    public int Id { get; set; }
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
}