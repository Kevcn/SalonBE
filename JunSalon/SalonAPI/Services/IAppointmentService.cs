using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SalonAPI.Domain;

namespace SalonAPI.Services
{
    public interface IAppointmentService
    {
        Task<List<DayAvailability>> GetDayAvailablity(DateTime date);
        Task<List<TimeAvailability>> GetTimeAvailablity(DateTime date);
        Task<bool> BookAppointment(BookingRecord bookingRecord);
        Task<bool> CancelAppointment(int bookingID);
        Task<List<BookingRecord>> GetAppointmentsByContact(Contact contactDetails);
        Task<List<BookingRecord>> GetAppointmentByDate(DateTime startDate, DateTime endDate);
    }
}