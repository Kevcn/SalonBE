using System;

namespace SalonAPI.Contracts.V1.Responses
{
    public class BookingResponse
    {
        public int ID { get; set; }
        public ContactResponse contact { get; set; }
        public int TimeSlotID { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}