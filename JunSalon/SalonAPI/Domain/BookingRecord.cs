using System;

namespace SalonAPI.Domain
{
    public class BookingRecord
    {
        public int ID { get; set; }
        public Contact contact { get; set; }
        public int TimeSlotID { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}