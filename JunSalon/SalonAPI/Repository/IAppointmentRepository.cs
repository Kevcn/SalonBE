using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SalonAPI.Domain;

namespace SalonAPI.Repository
{
    public interface IAppointmentRepository
    {
        Task<List<BookingRecord>> GetAppointmentsByDay(DateTime startDate, DateTime endDate);

        Task<List<BookingRecord>> GetSingleDayAppointments(DateTime date);

        Task<bool> AddAppointment(BookingRecord bookingRecord, int contactID);

        Task<bool> RemoveAppointment(BookingRecord bookingRecord);

        Task<BookingRecord> GetRecord(DateTime startDate, DateTime endDate);
    }
    
}