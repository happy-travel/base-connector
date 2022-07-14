using System.Net;
using CSharpFunctionalExtensions;
using HappyTravel.EdoContracts.Accommodations.Enums;
using HappyTravel.EdoContracts.Errors;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.BaseConnector.Api.Infrastructure.Helpers;

public static class ResultsHelper
{
    private static Result<T, ProblemDetails> CreateFailureResult<T>(string message, BookingFailureCodes code)
    {
        var details = new ProblemDetails
        {
            Detail = message,
            Status = (int) HttpStatusCode.BadRequest
        };
        details.Extensions.AddBookingFailureCode(code);
        
        return Result.Failure<T, ProblemDetails>(details);
    }
    
    
    private static Result<T, ProblemDetails> CreateFailureResult<T>(string message, SearchFailureCodes code)
    {
        var details = new ProblemDetails
        {
            Detail = message,
            Status = (int) HttpStatusCode.BadRequest
        };
        details.Extensions.AddSearchFailureCode(code);
        
        return Result.Failure<T, ProblemDetails>(details);
    }
}
