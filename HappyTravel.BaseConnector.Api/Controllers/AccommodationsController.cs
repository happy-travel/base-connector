using HappyTravel.BaseConnector.Api.Services.Accommodations;
using HappyTravel.EdoContracts.Accommodations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace HappyTravel.BaseConnector.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/{v:apiVersion}/accommodations")]
[Produces("application/json")]
public class AccommodationsController : BaseController
{
    public AccommodationsController(IAccommodationService accommodationService)
    {
        _accommodationService = accommodationService;
    }


    /// <summary>
    /// Returns a list of accommodation details.
    /// </summary>
    /// <param name="skip">Skip</param>
    /// <param name="top">Top</param>
    /// <param name="modificationDate">Last modification date</param>
    /// <param name="cancellationToken"></param>
    /// <returns>List of accommodations</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<MultilingualAccommodation>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get([FromQuery] int skip = 0, [FromQuery] int top = 0,
        [FromQuery(Name = "modification-date")]
            DateTime? modificationDate = null, CancellationToken cancellationToken = default)
    {
        return Ok(await _accommodationService.Get(skip, top, modificationDate, cancellationToken));
    }


    private readonly IAccommodationService _accommodationService;
}
