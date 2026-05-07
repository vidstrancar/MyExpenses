using Microsoft.EntityFrameworkCore;
using MyExpenses.Entities;
using MyExpenses.Models;

namespace MyExpenses.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<User> Users { get; set; }
}