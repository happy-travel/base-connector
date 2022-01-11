using CSharpFunctionalExtensions;
using HappyTravel.EdoContracts.Accommodations;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace HappyTravel.BaseConnector.Api.Services.Bookings;

public interface IBookingService
{
    Task<Result<Booking, ProblemDetails>> Book(BookingRequest bookingRequest, CancellationToken cancellationToken);
    Task<Result<Booking>> Get(string referenceCode, CancellationToken cancellationToken);
    Task<Result> Cancel(string referenceCode, CancellationToken cancellationToken);
}
