using System;

namespace SalonAPI.Domain
{
    public class BookingRecord
    {
        public Contact contact { get; set; }
        public AppointmentTime Time { get; set; }
        public DateTime Date { get; set; }
    }
}