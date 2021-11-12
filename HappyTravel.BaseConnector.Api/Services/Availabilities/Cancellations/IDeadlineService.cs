using CSharpFunctionalExtensions;
using HappyTravel.EdoContracts.Accommodations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HappyTravel.BaseConnector.Api.Services.Availabilities.Cancellations
{
    public interface IDeadlineService
    {
        Task<Result<Deadline>> Get(string availabilityId, Guid roomContractSetId, CancellationToken cancellationToken);
    }
}
