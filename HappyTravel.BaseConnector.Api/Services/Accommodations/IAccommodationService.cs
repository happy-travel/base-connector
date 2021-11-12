using HappyTravel.EdoContracts.Accommodations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HappyTravel.BaseConnector.Api.Services.Accommodations
{
    public interface IAccommodationService
    {
        Task<List<MultilingualAccommodation>> Get(int skip, int top, DateTime? modificationDate, CancellationToken cancellationToken);
    }
}
