namespace MyExpenses.Exceptions;

public class ExpensesServiceException: Exception
{
    public int StatusCode { get; } = StatusCodes.Status400BadRequest;
    
    public ExpensesServiceException(string message) : base(message) { }
    public ExpensesServiceException(string message, Exception innerException) : base(message, innerException) { }

    public ExpensesServiceException(string message, int statusCode, Exception innerException) : base(message,
        innerException)
    {
        StatusCode = statusCode;
    }
}