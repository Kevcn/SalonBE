using System.Collections.Generic;
using System.Threading.Tasks;
using SalonAPI.Domain;

namespace SalonAPI.Services
{
    public interface IBookingService
    {
        Task<List<DayAvailability>> GetDayAvailablity(string date);
        Task<List<TimeAvailability>> GetTimeAvailablity(string date);
        Task<bool> BookAppointment(BookingRecord bookingRecord);
        Task<bool> CancelAppointment(BookingRecord bookingRecord);
    }
}