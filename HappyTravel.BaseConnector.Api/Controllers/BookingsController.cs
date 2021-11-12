using CSharpFunctionalExtensions;
using HappyTravel.BaseConnector.Api.Infrastructure.Extensions;
using HappyTravel.BaseConnector.Api.Infrastructure.Logging;
using HappyTravel.BaseConnector.Api.Services.Bookings;
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
    public class BookingsController : BaseController
    {
        public BookingsController(IBookingService bookingService, ILogger<BookingsController> logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }


        /// <summary>
        /// Makes an asynchronous booking request to the supplier api.
        /// </summary>
        /// <param name="bookingRequest">Request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        [HttpPost("bookings")]
        [ProducesResponseType(typeof(Booking), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Book([FromBody] BookingRequest bookingRequest, CancellationToken cancellationToken)
        {
            using var bookingReferenceCodeScope = _logger.AddScopedValue("BookingReferenceCode", bookingRequest.ReferenceCode);
            _logger.LogBookingRequestStarted();

            var (isSuccess, _, availability, error) = await _bookingService.Book(bookingRequest, cancellationToken);
            if (isSuccess)
            {
                _logger.LogBookingRequestCompleted();
                return Ok(availability);
            }

            _logger.LogBookingRequestFailed(error.Detail);
            return BadRequest(error);
        }


        /// <summary>
        /// Makes a cancellation request to the supplier api
        /// </summary>
        /// <param name="referenceCode">Booking reference code</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        [HttpPost("bookings/{referenceCode}/cancel")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Cancel(string referenceCode, CancellationToken cancellationToken)
        {
            using var bookingReferenceCodeScope = _logger.AddScopedValue("BookingReferenceCode", referenceCode);
            _logger.LogCancelBookingRequestStarted();

            var (isSuccess, _, error) = await _bookingService.Cancel(referenceCode, cancellationToken);
            if (isSuccess)
            {
                _logger.LogCancelBookingRequestCompleted();
                return Ok();
            }

            _logger.LogCancelBookingRequestFailed(error);
            return BadRequestWithProblemDetails(error);
        }


        /// <summary>
        /// Returns information about a booking order.
        /// </summary>
        /// <param name="referenceCode">Booking reference code</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        [HttpGet("bookings/{referenceCode}")]
        [ProducesResponseType(typeof(Booking), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetDetails(string referenceCode, CancellationToken cancellationToken)
        {
            using var bookingReferenceCodeScope = _logger.AddScopedValue("BookingReferenceCode", referenceCode);
            _logger.LogBookingStatusRequestStarted();

            var (isSuccess, _, booking, error) = await _bookingService.Get(referenceCode, cancellationToken);
            if (isSuccess)
            {
                _logger.LogBookingStatusRequestCompleted();
                return Ok(booking);
            }

            _logger.LogBookingStatusRequestFailed(error);
            return BadRequestWithProblemDetails(error);
        }


        private readonly IBookingService _bookingService;
        private readonly ILogger<BookingsController> _logger;
    }
}
