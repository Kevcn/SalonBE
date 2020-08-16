using System;

namespace SalonAPI.Contracts.V1.Requests
{
    public class BookingRecordRequest
    {
        public ContactRequest contact { get; set; }
        public int TimeSlotID { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}