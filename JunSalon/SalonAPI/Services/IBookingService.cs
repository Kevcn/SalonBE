using System.Threading.Tasks;
using SalonAPI.Domain;

namespace SalonAPI.Services
{
    public interface IBookingService
    {
        Task<DayAvailability> GetDayAvailablity(string date);
        Task<TimeAvailability> GetTimeAvailablity(string date);
        Task<bool> BookAppointment(BookingRecord bookingRecord);
        Task<bool> CancelAppointment(BookingRecord bookingRecord);
    }
}