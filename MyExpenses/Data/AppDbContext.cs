using Microsoft.EntityFrameworkCore;
using MyExpenses.Models;

namespace MyExpenses.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Expense> Expenses { get; set; }
}