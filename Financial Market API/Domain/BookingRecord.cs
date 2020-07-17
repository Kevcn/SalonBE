using System;

namespace Financial_Market_API.Domain
{
    public class BookingRecord
    {
        public Contact contact { get; set; }
        public AppointmentTime Time { get; set; }
        public DateTime Date { get; set; }
    }
}