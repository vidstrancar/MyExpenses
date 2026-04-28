namespace MyExpenses.Exceptions;

public class CategoriesServiceException: Exception
{
    public int StatusCode { get; } = StatusCodes.Status400BadRequest;
    
    public CategoriesServiceException(string message) : base(message) { }
    public CategoriesServiceException(string message, Exception innerException) : base(message, innerException) { }

    public CategoriesServiceException(string message, int statusCode, Exception innerException) : base(message,
        innerException)
    {
        StatusCode = statusCode;
    }
    
}