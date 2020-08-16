using System;
using System.Collections.Generic;
using System.Text;

namespace SalonAPI.Contracts.V1
{
    public static class ApiRoutes
    {
        // Base URL
        private const string Root = "api";
        private const string Version = "v1";
        private const string Base = Root + "/" + Version;
        
        public static class Appointment
        {
            public const string GetDayavailability = Base + "/GetDayavailability/{date}";

            public const string GetTimeavailability = Base + "/GetTimeavailability/{date}";

            public const string Book = Base + "/Book";

            public const string Get = Base + "/Get/{bookingID}";
            
            public const string Cancel = Base + "/Cancel";
            
            public const string GetByDate = Base + "/GetByDate";
            
            public const string GetByContact = Base + "/GetByContact";
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
