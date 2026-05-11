namespace MyExpenses.Services;

public interface IUserContext
{
    public Guid GetUserId { get; }
}