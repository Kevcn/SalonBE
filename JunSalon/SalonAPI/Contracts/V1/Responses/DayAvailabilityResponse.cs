using System;

namespace SalonAPI.Contracts.V1.Responses
{
    public class DayAvailabilityResponse
    {
        public DateTime date { get; set; }
        public bool Available { get; set; }
    }
}