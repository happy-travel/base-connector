using CSharpFunctionalExtensions;
using HappyTravel.EdoContracts.Accommodations;
using System.Threading;
using System.Threading.Tasks;

namespace HappyTravel.BaseConnector.Api.Services.Availabilities.WideAvailabilities;

public interface IWideAvailabilitySearchService
{
    Task<Result<Availability>> Get(AvailabilityRequest request, string languageCode, CancellationToken cancellationToken);
}
