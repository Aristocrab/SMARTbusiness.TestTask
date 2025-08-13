using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace SMARTbusiness.TestTask.WebApi.Extensions;

public static class ResultExtensions
{
    public static ActionResult<T> ToActionResult<T>(this ErrorOr<T> result)
    {
        if (!result.IsError)
            return result.Value;

        return result.FirstError.ToProblemDetails();
    }
        
    private static ObjectResult ToProblemDetails(this Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError,
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = error.Description
        };

        return new ObjectResult(problemDetails) { StatusCode = statusCode };
    }
}