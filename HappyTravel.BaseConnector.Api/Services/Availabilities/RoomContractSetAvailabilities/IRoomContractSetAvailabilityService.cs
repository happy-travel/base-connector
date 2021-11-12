using CSharpFunctionalExtensions;
using HappyTravel.EdoContracts.Accommodations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HappyTravel.BaseConnector.Api.Services.Availabilities.RoomContractSetAvailabilities
{
    public interface IRoomContractSetAvailabilityService
    {
        Task<Result<RoomContractSetAvailability>> Get(string availabilityId, Guid roomContractSetId, CancellationToken cancellationToken);
    }
}
