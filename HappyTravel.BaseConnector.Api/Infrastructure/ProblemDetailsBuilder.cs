using CSharpFunctionalExtensions;
using HappyTravel.EdoContracts.Accommodations.Enums;
using HappyTravel.EdoContracts.Errors;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HappyTravel.BaseConnector.Api.Infrastructure;

public static class ProblemDetailsBuilder
{
    public static Result<T, ProblemDetails> CreateFailureResult<T>(string message, BookingFailureCodes code)
    {
        var details = new ProblemDetails
        {
            Detail = message,
            Status = (int)HttpStatusCode.BadRequest
        };
        details.Extensions.AddBookingFailureCode(code);

        return Result.Failure<T, ProblemDetails>(details);
    }
}
