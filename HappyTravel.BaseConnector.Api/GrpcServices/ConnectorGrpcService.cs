using System.Globalization;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.BaseConnector.Api.Infrastructure.Extensions;
using HappyTravel.BaseConnector.Api.Infrastructure.Logging;
using HappyTravel.BaseConnector.Api.Services.Availabilities.AccommodationAvailabilities;
using HappyTravel.BaseConnector.Api.Services.Availabilities.Cancellations;
using HappyTravel.BaseConnector.Api.Services.Availabilities.RoomContractSetAvailabilities;
using HappyTravel.BaseConnector.Api.Services.Availabilities.WideAvailabilities;
using HappyTravel.BaseConnector.Api.Services.Bookings;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.EdoContracts.Accommodations.Enums;
using HappyTravel.EdoContracts.Grpc.Models;
using HappyTravel.EdoContracts.Grpc.Services;
using HappyTravel.EdoContracts.Grpc.Surrogates;
using HappyTravel.GrpcResultContract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;

namespace HappyTravel.BaseConnector.Api.GrpcServices;

public class ConnectorGrpcService : IConnectorGrpcService
{
    public ConnectorGrpcService(IWideAvailabilitySearchService wideAvailabilityService, IBookingService bookingService, 
        ILogger<ConnectorGrpcService> logger, IAccommodationAvailabilityService accommodationAvailabilityService, IRoomContractSetAvailabilityService roomContractSetAvailabilityService, IDeadlineService deadlineService)
    {
        _wideAvailabilityService = wideAvailabilityService;
        _bookingService = bookingService;
        _logger = logger;
        _accommodationAvailabilityService = accommodationAvailabilityService;
        _roomContractSetAvailabilityService = roomContractSetAvailabilityService;
        _deadlineService = deadlineService;
    }
    
    
    public async Task<WideAvailabilityResponse> GetWideAvailability(AvailabilityRequestSurrogate request, CallContext context)
    {
        using var accommodationsScope = _logger.AddScopedValue("Accommodations", string.Join(',', request.AccommodationIds));
        using var roomsCountScope = _logger.AddScopedValue("RoomsCount", request.Rooms.Count);
        _logger.LogSearchRequestStarted();
            
        var (_, isFailure, result, error) = await _wideAvailabilityService.Get(request, LanguageCode, context.CancellationToken);
        if (isFailure)
            _logger.LogSearchRequestFailed(error);
        
        _logger.LogSearchRequestCompleted();

        return new()
        {
            Result = isFailure
                ? GrpcResult<Availability, string>.Failure(error)
                : GrpcResult<Availability, string>.Success(result)
        };
    }


    public async Task<AccommodationAvailabilityResponse> GetAccommodationAvailability(AccommodationAvailabilityRequest request, CallContext context)
    {
        using var accommodationScope = _logger.AddScopedValue("AccommodationId", request.AccommodationId);
        _logger.LogAccommodationRequestStarted(); 
            
        var (_, isFailure, result, error) = await _accommodationAvailabilityService.Get(request.AvailabilityId, request.AccommodationId, context.CancellationToken);
        if (isFailure)
            _logger.LogAccommodationRequestFailed(error.Detail);

        _logger.LogAccommodationRequestCompleted();
        
        return new()
        {
            Result = isFailure
                ? GrpcResult<AccommodationAvailability, ProblemDetails>.Failure(error)
                : GrpcResult<AccommodationAvailability, ProblemDetails>.Success(result)
        };
    }


    public async Task<ExactAvailabilityResponse> GetExactAvailability(ExactAvailabilityRequest request, CallContext context)
    {
        using var roomContractSetScope = _logger.AddScopedValue("RoomContractSetId", request.RoomContractSetId);
        _logger.LogRoomRequestStarted();
            
        var (_, isFailure, result, error) = await _roomContractSetAvailabilityService.Get(request.AvailabilityId, request.RoomContractSetId, context.CancellationToken);
        if (isFailure)
            _logger.LogRoomRequestFailed(error);

        _logger.LogRoomRequestCompleted();

        return new()
        {
            Result = isFailure
                ? GrpcResult<RoomContractSetAvailability?, string>.Failure(error)
                : GrpcResult<RoomContractSetAvailability?, string>.Success(result)
        };
    }


    public async Task<DeadlineResponse> GetDeadline(DeadlineRequest request, CallContext context)
    {
        _logger.LogDeadlineRequestStarted();

        var (_, isFailure, deadline, error) = await _deadlineService.Get(request.AvailabilityId, request.RoomContractSetId, context.CancellationToken);
        if(isFailure)
            _logger.LogDeadlineRequestFailed(error);
        
        _logger.LogDeadlineRequestCompleted();

        return new()
        {
            Result = isFailure
                ? GrpcResult<Deadline, string>.Failure(error)
                : GrpcResult<Deadline, string>.Success(deadline)
        };
    }


    public async Task<BookingResponse> Book(BookingRequestSurrogate request, CallContext context)
    {
        using var bookingReferenceCodeScope = _logger.AddScopedValue("BookingReferenceCode", request.ReferenceCode);
        _logger.LogBookingRequestStarted();
            
        var (_, isFailure, bookingDetails, error) = await _bookingService.Book(request, context.CancellationToken);
        if (isFailure)
            _logger.LogBookingRequestFailed(error.Detail);

        _logger.LogBookingRequestCompleted();

        return new()
        {
            Result = isFailure
                ? GrpcResult<Booking, ProblemDetails>.Failure(error)
                : GrpcResult<Booking, ProblemDetails>.Success(bookingDetails)
        };
    }


    public async Task<CancelBookingResponse> CancelBooking(CancelBookingRequest request, CallContext context)
    {
        using var bookingReferenceCodeScope = _logger.AddScopedValue("BookingReferenceCode", request.ReferenceCode);
        _logger.LogCancelBookingRequestStarted();
            
        var (_, isFailure, error) = await _bookingService.Cancel(request.ReferenceCode, context.CancellationToken);
        if (isFailure)
            _logger.LogCancelBookingRequestFailed(error);

        _logger.LogCancelBookingRequestCompleted();

        return new()
        {
            // TODO: think how to return empty result
            Result = isFailure
                ? GrpcResult<BookingStatusCodes, string>.Failure(error)
                : GrpcResult<BookingStatusCodes, string>.Success(BookingStatusCodes.Cancelled)
        };
    }


    public async Task<BookingInfoResponse> GetBooking(BookingInfoRequest request, CallContext context)
    {
        using var bookingReferenceCodeScope = _logger.AddScopedValue("BookingReferenceCode", request.ReferenceCode);
        _logger.LogBookingStatusRequestStarted();
            
        var (_, isFailure, bookingDetails, error) = await _bookingService.Get(request.ReferenceCode, context.CancellationToken);
        if (isFailure)
            _logger.LogBookingStatusRequestFailed(error);

        _logger.LogBookingStatusRequestCompleted();

        return new()
        {
            Result = isFailure
                ? GrpcResult<Booking, string>.Failure(error)
                : GrpcResult<Booking, string>.Success(bookingDetails)
        };
    }


    private static string LanguageCode => CultureInfo.CurrentCulture.Name;
    
    
    private readonly IWideAvailabilitySearchService _wideAvailabilityService;
    private readonly IAccommodationAvailabilityService _accommodationAvailabilityService;
    private readonly IRoomContractSetAvailabilityService _roomContractSetAvailabilityService;
    private readonly IBookingService _bookingService;
    private readonly IDeadlineService _deadlineService;
    private readonly ILogger<ConnectorGrpcService> _logger;
}