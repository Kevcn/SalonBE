using System;
using Contracts.V1.Requests;

namespace Contracts.V1.Responses
{
    public class BookingResponse
    {
        public ContactResponse contact { get; set; }
        public int TimeSlotID { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}