using CSharpFunctionalExtensions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.BaseConnector.Api.Services.Availabilities.AccommodationAvailabilities;

public interface IAccommodationAvailabilityService
{
    Task<Result<EdoContracts.Accommodations.AccommodationAvailability, ProblemDetails>> Get(string availabilityId, string accommodationId, CancellationToken cancellationToken);
}
