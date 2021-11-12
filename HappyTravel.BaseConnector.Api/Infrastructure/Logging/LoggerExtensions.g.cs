using System;
using Microsoft.Extensions.Logging;

namespace HappyTravel.BaseConnector.Api.Infrastructure.Logging
{
    public static class LoggerExtensions
    {
        static LoggerExtensions()
        {
            BookingRequestStarted = LoggerMessage.Define(LogLevel.Information,
                new EventId(3011, "BookingRequestStarted"),
                "Booking request started");
            
            BookingRequestCompleted = LoggerMessage.Define(LogLevel.Information,
                new EventId(3012, "BookingRequestCompleted"),
                "Booking request completed");
            
            BookingRequestFailed = LoggerMessage.Define<string>(LogLevel.Warning,
                new EventId(3013, "BookingRequestFailed"),
                "Booking request failed with error `{Error}`");
            
            CancelBookingRequestStarted = LoggerMessage.Define(LogLevel.Information,
                new EventId(3014, "CancelBookingRequestStarted"),
                "Cancel booking request started");
            
            CancelBookingRequestCompleted = LoggerMessage.Define(LogLevel.Information,
                new EventId(3015, "CancelBookingRequestCompleted"),
                "Cancel booking request completed");
            
            CancelBookingRequestFailed = LoggerMessage.Define<string>(LogLevel.Warning,
                new EventId(3016, "CancelBookingRequestFailed"),
                "Cancel booking request failed with error `{Error}`");
            
            BookingStatusRequestStarted = LoggerMessage.Define(LogLevel.Information,
                new EventId(3017, "BookingStatusRequestStarted"),
                "Booking status request started");
            
            BookingStatusRequestCompleted = LoggerMessage.Define(LogLevel.Information,
                new EventId(3018, "BookingStatusRequestCompleted"),
                "Booking status request completed");
            
            BookingStatusRequestFailed = LoggerMessage.Define<string>(LogLevel.Warning,
                new EventId(3019, "BookingStatusRequestFailed"),
                "Booking status request failed with error `{Error}`");
            
            SearchRequestStarted = LoggerMessage.Define(LogLevel.Information,
                new EventId(3020, "SearchRequestStarted"),
                "Search request started");
            
            SearchRequestCompleted = LoggerMessage.Define(LogLevel.Information,
                new EventId(3021, "SearchRequestCompleted"),
                "Search request completed");
            
            SearchRequestFailed = LoggerMessage.Define<string>(LogLevel.Warning,
                new EventId(3022, "SearchRequestFailed"),
                "Search request failed with error `{Error}`");
            
            AccommodationRequestStarted = LoggerMessage.Define(LogLevel.Information,
                new EventId(3023, "AccommodationRequestStarted"),
                "Accommodation request started");
            
            AccommodationRequestCompleted = LoggerMessage.Define(LogLevel.Information,
                new EventId(3024, "AccommodationRequestCompleted"),
                "Accommodation request completed");
            
            AccommodationRequestFailed = LoggerMessage.Define<string>(LogLevel.Warning,
                new EventId(3025, "AccommodationRequestFailed"),
                "Accommodation request failed with error `{Error}`");
            
            RoomRequestStarted = LoggerMessage.Define(LogLevel.Information,
                new EventId(3026, "RoomRequestStarted"),
                "Room request started");
            
            RoomRequestCompleted = LoggerMessage.Define(LogLevel.Information,
                new EventId(3027, "RoomRequestCompleted"),
                "Room request completed");
            
            RoomRequestFailed = LoggerMessage.Define<string>(LogLevel.Warning,
                new EventId(3028, "RoomRequestFailed"),
                "Room request failed with error `{Error}`");
            
            DeadlineRequestStarted = LoggerMessage.Define(LogLevel.Information,
                new EventId(3032, "DeadlineRequestStarted"),
                "Deadline request started");
            
            DeadlineRequestCompleted = LoggerMessage.Define(LogLevel.Information,
                new EventId(3033, "DeadlineRequestCompleted"),
                "Deadline request completed");
            
            DeadlineRequestFailed = LoggerMessage.Define<string>(LogLevel.Warning,
                new EventId(3034, "DeadlineRequestFailed"),
                "Deadline request failed with error `{Error}`");
            
        }
    
                
         public static void LogBookingRequestStarted(this ILogger logger, Exception exception = null)
            => BookingRequestStarted(logger, exception);
                
         public static void LogBookingRequestCompleted(this ILogger logger, Exception exception = null)
            => BookingRequestCompleted(logger, exception);
                
         public static void LogBookingRequestFailed(this ILogger logger, string Error, Exception exception = null)
            => BookingRequestFailed(logger, Error, exception);
                
         public static void LogCancelBookingRequestStarted(this ILogger logger, Exception exception = null)
            => CancelBookingRequestStarted(logger, exception);
                
         public static void LogCancelBookingRequestCompleted(this ILogger logger, Exception exception = null)
            => CancelBookingRequestCompleted(logger, exception);
                
         public static void LogCancelBookingRequestFailed(this ILogger logger, string Error, Exception exception = null)
            => CancelBookingRequestFailed(logger, Error, exception);
                
         public static void LogBookingStatusRequestStarted(this ILogger logger, Exception exception = null)
            => BookingStatusRequestStarted(logger, exception);
                
         public static void LogBookingStatusRequestCompleted(this ILogger logger, Exception exception = null)
            => BookingStatusRequestCompleted(logger, exception);
                
         public static void LogBookingStatusRequestFailed(this ILogger logger, string Error, Exception exception = null)
            => BookingStatusRequestFailed(logger, Error, exception);
                
         public static void LogSearchRequestStarted(this ILogger logger, Exception exception = null)
            => SearchRequestStarted(logger, exception);
                
         public static void LogSearchRequestCompleted(this ILogger logger, Exception exception = null)
            => SearchRequestCompleted(logger, exception);
                
         public static void LogSearchRequestFailed(this ILogger logger, string Error, Exception exception = null)
            => SearchRequestFailed(logger, Error, exception);
                
         public static void LogAccommodationRequestStarted(this ILogger logger, Exception exception = null)
            => AccommodationRequestStarted(logger, exception);
                
         public static void LogAccommodationRequestCompleted(this ILogger logger, Exception exception = null)
            => AccommodationRequestCompleted(logger, exception);
                
         public static void LogAccommodationRequestFailed(this ILogger logger, string Error, Exception exception = null)
            => AccommodationRequestFailed(logger, Error, exception);
                
         public static void LogRoomRequestStarted(this ILogger logger, Exception exception = null)
            => RoomRequestStarted(logger, exception);
                
         public static void LogRoomRequestCompleted(this ILogger logger, Exception exception = null)
            => RoomRequestCompleted(logger, exception);
                
         public static void LogRoomRequestFailed(this ILogger logger, string Error, Exception exception = null)
            => RoomRequestFailed(logger, Error, exception);
                
         public static void LogDeadlineRequestStarted(this ILogger logger, Exception exception = null)
            => DeadlineRequestStarted(logger, exception);
                
         public static void LogDeadlineRequestCompleted(this ILogger logger, Exception exception = null)
            => DeadlineRequestCompleted(logger, exception);
                
         public static void LogDeadlineRequestFailed(this ILogger logger, string Error, Exception exception = null)
            => DeadlineRequestFailed(logger, Error, exception);
    
    
        
        private static readonly Action<ILogger, Exception> BookingRequestStarted;
        
        private static readonly Action<ILogger, Exception> BookingRequestCompleted;
        
        private static readonly Action<ILogger, string, Exception> BookingRequestFailed;
        
        private static readonly Action<ILogger, Exception> CancelBookingRequestStarted;
        
        private static readonly Action<ILogger, Exception> CancelBookingRequestCompleted;
        
        private static readonly Action<ILogger, string, Exception> CancelBookingRequestFailed;
        
        private static readonly Action<ILogger, Exception> BookingStatusRequestStarted;
        
        private static readonly Action<ILogger, Exception> BookingStatusRequestCompleted;
        
        private static readonly Action<ILogger, string, Exception> BookingStatusRequestFailed;
        
        private static readonly Action<ILogger, Exception> SearchRequestStarted;
        
        private static readonly Action<ILogger, Exception> SearchRequestCompleted;
        
        private static readonly Action<ILogger, string, Exception> SearchRequestFailed;
        
        private static readonly Action<ILogger, Exception> AccommodationRequestStarted;
        
        private static readonly Action<ILogger, Exception> AccommodationRequestCompleted;
        
        private static readonly Action<ILogger, string, Exception> AccommodationRequestFailed;
        
        private static readonly Action<ILogger, Exception> RoomRequestStarted;
        
        private static readonly Action<ILogger, Exception> RoomRequestCompleted;
        
        private static readonly Action<ILogger, string, Exception> RoomRequestFailed;
        
        private static readonly Action<ILogger, Exception> DeadlineRequestStarted;
        
        private static readonly Action<ILogger, Exception> DeadlineRequestCompleted;
        
        private static readonly Action<ILogger, string, Exception> DeadlineRequestFailed;
    }
}