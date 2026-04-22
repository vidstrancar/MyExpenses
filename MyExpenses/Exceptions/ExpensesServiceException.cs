namespace MyExpenses.Exceptions;

public class ExpensesServiceException: Exception
{
    public ExpensesServiceException(string message) : base(message) { }
    public ExpensesServiceException(string message, Exception innerException) : base(message, innerException) { }
}