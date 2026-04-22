using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyExpenses.Exceptions;

namespace MyExpenses.Infrastructure;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger): IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);
        
        var (statusCode, title) = exception switch {
            ExpensesServiceException ex => (ex.StatusCode, "Service Error"),
            _ => (StatusCodes.Status500InternalServerError, "Server Error")
        };

        var problemDetails = new ProblemDetails()
        {
            Status = statusCode,
            Title = title,
            Detail = exception.Message,
            Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        
        return true;
    }
}