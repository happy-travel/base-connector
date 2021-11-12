using CSharpFunctionalExtensions;
using System.Threading;
using System.Threading.Tasks;

namespace HappyTravel.BaseConnector.Api.Services.Availabilities.AccommodationAvailabilities
{
    public interface IAccommodationAvailabilityService
    {
        Task<Result<EdoContracts.Accommodations.AccommodationAvailability>> Get(string availabilityId, string accommodationId, CancellationToken cancellationToken);
    }
}
