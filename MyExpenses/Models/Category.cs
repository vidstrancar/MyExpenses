using MyExpenses.Entities;

namespace MyExpenses.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? ParentId { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
}