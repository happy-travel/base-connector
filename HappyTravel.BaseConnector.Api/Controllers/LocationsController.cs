using HappyTravel.BaseConnector.Api.Services.Locations;
using HappyTravel.EdoContracts.GeoData;
using HappyTravel.EdoContracts.GeoData.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace HappyTravel.BaseConnector.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/{v:apiVersion}/locations")]
[Produces("application/json")]
public class LocationsController : Controller
{
    public LocationsController(ILocationService locationService)
    {
        _locationService = locationService;
    }


    /// <summary>
    /// Gets locations.
    /// </summary>
    /// <param name="modified">Last modified date</param>
    /// <param name="locationType">Location type</param>
    /// <param name="skip">Skip count</param>
    /// <param name="take">Take count</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Location list</returns>
    [HttpGet("{modified}")]
    [ProducesResponseType(typeof(List<Location>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetLocations([FromRoute] DateTime modified, [FromQuery] LocationTypes locationType, [FromQuery] int skip = 0,
        [FromQuery] int take = 10000, CancellationToken cancellationToken = default)
    {
        return Ok(await _locationService.Get(modified, locationType, skip, take, cancellationToken));
    }


    private readonly ILocationService _locationService;
}
