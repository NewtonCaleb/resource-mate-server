using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SocialWorkApi.API.Infrastructure.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ProblemDetails problemDetails = GetProblemDetails(exception);
        httpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
        await httpContext
            .Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }

    static private ProblemDetails GetProblemDetails(Exception ex)
    {
        if (ex.GetType().Name == "ValidationException")
        {
            return new ProblemDetails
            {
                Status = (int)System.Net.HttpStatusCode.BadRequest,
                Type = ex.GetType().Name,
                Title = "Data Validation Failed",
                Detail = String.Join("\n", ex.Message.Split("--")[1..].Select((value) => value.Split("Severity")[0].Trim()))
            };
        }

        return new ProblemDetails
            {
                Status = (int)System.Net.HttpStatusCode.InternalServerError,
                Type = ex.GetType().Name,
                Title = "An unhandled error occurred",
                Detail = ex.Message
            };
    }
}