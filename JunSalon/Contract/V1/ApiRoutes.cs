using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.V1
{
    public static class ApiRoutes
    {
        // Base URL
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;

        // Routes for Stocks // TODO: To be removed
        public static class Stock
        {
            // GET
            public const string GetAll = Base + "/stocks";

            public const string Update = Base + "/stocks/{stockId}";

            public const string Delete = Base + "/stocks/{stockId}";

            public const string Get = Base + "/stocks/{stockId}";

            // POST
            public const string Create = Base + "/stocks";
        }

        public static class Appointment
        {
            public const string GetDayAvailablity = Base + "/GetDayAvailablity/{date}";

            public const string GetTimeAvailablity = Base + "/GetTimeAvailablity/{date}";

            public const string Book = Base + "/Book";

            public const string Get = Base + "/Book/{bookingID}";
            
            public const string Cancel = Base + "/Cancel";
            
            public const string ViewBooking = Base + "/ViewBooking";
            
            public const string GetAppointment = Base + "/GetAppointment";
        }

        // Not quite RESTful standard
        public static class Identity
        {
            public const string Login = Base + "/identity/login";

            public const string Register = Base + "/identity/register";

            public const string Refresh = Base + "/identity/refresh";
        }
    }
}
