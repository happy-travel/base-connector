using HappyTravel.EdoContracts.Accommodations.Enums;
using HappyTravel.EdoContracts.Errors;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.BaseConnector.Api.Infrastructure.Extensions;

public static class ProblemDetailsExtensions
{
    /// <summary>
    /// Adds booking failure code to extensions of ProblemDetails
    /// </summary>
    /// <param name="problemDetails">ProblemDetails to add to booking failure code</param>
    /// <param name="code">Booking failure code</param>
    /// <returns>ProblemDetails with BookingFailureCode in extensions</returns>
    public static ProblemDetails AddBookingFailureCode(this ProblemDetails problemDetails, BookingFailureCodes code)
    {
        problemDetails.Extensions.AddBookingFailureCode(code);
        return problemDetails;
    }


    /// <summary>
    /// Adds search failure code to extensions of ProblemDetails
    /// </summary>
    /// <param name="problemDetails">ProblemDetails to add to search failure code</param>
    /// <param name="code">Search failure code</param>
    /// <returns>ProblemDetails with SearchFailureCode in extensions</returns>
    public static ProblemDetails AddSearchFailureCodes(this ProblemDetails problemDetails, SearchFailureCodes code)
    {
        problemDetails.Extensions.AddSearchFailureCode(code);
        return problemDetails;
    }
}
