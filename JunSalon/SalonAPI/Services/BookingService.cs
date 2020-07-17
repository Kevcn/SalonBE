using System.Collections.Generic;
using System.Threading.Tasks;
using SalonAPI.Domain;

namespace SalonAPI.Services
{
    public class BookingService : IBookingService
    {
        public async Task<List<DayAvailability>> GetDayAvailablity(string date)
        {
            // Query booking record on date + 14, if not all time slots are taken, return true
            
            throw new System.NotImplementedException();
        }

        public async Task<List<TimeAvailability>> GetTimeAvailablity(string date)
        {
            // query where date = date, get a list of bookingRecords, turn that into a list timeAvailable
            
            throw new System.NotImplementedException();
        }

        public async Task<bool> BookAppointment(BookingRecord bookingRecord)
        {
            // insert into bookingRecord
            
            // insert into contact - check for duplicates before insert
            
            throw new System.NotImplementedException();
        }

        public async Task<bool> CancelAppointment(BookingRecord bookingRecord)
        {
            // delete from bookingRecord
            
            // check date is in the future
            
            throw new System.NotImplementedException();
        }
    }
}