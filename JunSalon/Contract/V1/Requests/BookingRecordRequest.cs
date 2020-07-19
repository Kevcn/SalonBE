using System;

namespace Contracts.V1.Requests
{
    public class BookingRecordRequest
    {
        public Contact contact { get; set; }
        public int TimeSlotID { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}