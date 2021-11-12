using CSharpFunctionalExtensions;
using HappyTravel.BaseConnector.Api.Infrastructure.Extensions;
using HappyTravel.BaseConnector.Api.Infrastructure.Logging;
using HappyTravel.BaseConnector.Api.Services.Availabilities.AccommodationAvailabilities;
using HappyTravel.BaseConnector.Api.Services.Availabilities.Cancellations;
using HappyTravel.BaseConnector.Api.Services.Availabilities.RoomContractSetAvailabilities;
using HappyTravel.BaseConnector.Api.Services.Availabilities.WideAvailabilities;
using HappyTravel.EdoContracts.Accommodations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace HappyTravel.BaseConnector.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/accommodations")]
    [Produces("application/json")]
    public class AvailabilityController : BaseController
    {
        public AvailabilityController(IWideAvailabilitySearchService wideAvailabilitySearchService,
            IAccommodationAvailabilityService accommodationAvailabilityService,
            IRoomContractSetAvailabilityService roomContractSetAvailabilityService,
            IDeadlineService deadlineService, ILogger<AvailabilityController> logger)
        {
            _wideAvailabilitySearchService = wideAvailabilitySearchService;
            _accommodationAvailabilityService = accommodationAvailabilityService;
            _roomContractSetAvailabilityService = roomContractSetAvailabilityService;
            _deadlineService = deadlineService;
            _logger = logger;
        }

        /// <summary>
        /// Searches for accommodations with available room contract sets. The 1st search step.
        /// </summary>
        /// <param name="request">Availability search request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        [HttpPost("availabilities")]
        [ProducesResponseType(typeof(Availability), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get([FromBody] AvailabilityRequest request, CancellationToken cancellationToken)
        {
            using var accommodationsScope = _logger.AddScopedValue("Accommodations", string.Join(',', request.AccommodationIds));
            using var roomsCountScope = _logger.AddScopedValue("RoomsCount", request.Rooms.Count);
            _logger.LogSearchRequestStarted();

            var (isSuccess, _, availability, error) = await _wideAvailabilitySearchService.Get(request, LanguageCode, cancellationToken);
            if (isSuccess)
            {
                _logger.LogSearchRequestCompleted();
                return Ok(availability);
            }

            _logger.LogSearchRequestFailed(error);
            return BadRequestWithProblemDetails(error);
        }


        /// <summary>
        /// Searches for specific accommodation with available room contract sets. The 2nd search step. 
        /// </summary>
        /// <param name="accommodationId">Supplier accommodation id</param>
        /// <param name="availabilityId">Availability Id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        [HttpPost("{accommodationId}/availabilities/{availabilityId}")]
        [ProducesResponseType(typeof(AccommodationAvailability), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get([FromRoute] string accommodationId, [FromRoute] string availabilityId, CancellationToken cancellationToken)
        {
            using var accommodationScope = _logger.AddScopedValue("AccommodationId", accommodationId);
            _logger.LogAccommodationRequestStarted();

            var (isSuccess, _, availability, error) = await _accommodationAvailabilityService.Get(availabilityId, accommodationId, cancellationToken);
            if (isSuccess)
            {
                _logger.LogAccommodationRequestCompleted();
                return Ok(availability);
            }

            _logger.LogAccommodationRequestFailed(error);
            return BadRequestWithProblemDetails(error);
        }


        /// <summary>
        /// Searches exact accommodation with specific room contract sets.
        /// </summary>
        /// <param name="availabilityId">Availability Id</param>
        /// <param name="roomContractSetId">Id of selected room contract set</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("availabilities/{availabilityId}/room-contract-sets/{roomContractSetId}")]
        [ProducesResponseType(typeof(RoomContractSetAvailability), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get([FromRoute] string availabilityId, [FromRoute] Guid roomContractSetId, CancellationToken cancellationToken)
        {
            _logger.LogRoomRequestStarted();

            var (isSuccess, _, availability, error) = await _roomContractSetAvailabilityService.Get(availabilityId, roomContractSetId, cancellationToken);
            if (isSuccess)
            {
                _logger.LogRoomRequestCompleted();
                return Ok(availability);
            }

            _logger.LogRoomRequestFailed(error);
            return BadRequestWithProblemDetails(error);
        }


        /// <summary>
        /// Retrieves deadline data
        /// </summary>
        /// <param name="availabilityId"></param>
        /// <param name="roomContractSetId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("availabilities/{availabilityId}/room-contract-sets/{roomContractSetId}/deadline")]
        [ProducesResponseType(typeof(Deadline), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetDeadline([FromRoute] string availabilityId, [FromRoute] Guid roomContractSetId, CancellationToken cancellationToken)
        {
            _logger.LogDeadlineRequestStarted();

            var (isSuccess, _, availability, error) = await _deadlineService.Get(availabilityId, roomContractSetId, cancellationToken);
            if (isSuccess)
            {
                _logger.LogDeadlineRequestCompleted();
                return Ok(availability);
            }

            _logger.LogDeadlineRequestFailed(error);
            return BadRequestWithProblemDetails(error);
        }


        private readonly IWideAvailabilitySearchService _wideAvailabilitySearchService;
        private readonly IAccommodationAvailabilityService _accommodationAvailabilityService;
        private readonly IRoomContractSetAvailabilityService _roomContractSetAvailabilityService;
        private readonly IDeadlineService _deadlineService;
        private readonly ILogger<AvailabilityController> _logger;
    }
}
