using HappyTravel.EdoContracts.GeoData;
using HappyTravel.EdoContracts.GeoData.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HappyTravel.BaseConnector.Api.Services.Locations
{
    public interface ILocationService
    {
        Task<List<Location>> Get(DateTime modified, LocationTypes locationType, int skip, int top, CancellationToken cancellationToken);
    }
}
