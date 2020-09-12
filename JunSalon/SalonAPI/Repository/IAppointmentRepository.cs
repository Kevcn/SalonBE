using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SalonAPI.Domain;

namespace SalonAPI.Repository
{
    public interface IAppointmentRepository
    {
        // Check availability 
        Task<List<BookingRecord>> GetAppointmentsByDay(DateTime startDate, DateTime endDate);
        Task<List<BookingRecord>> GetSingleDayAppointments(DateTime date);

        // Book appointment
        Task<bool> VerifyTimeSlotAvailable(BookingRecord bookingRecord);
        Task<bool> AddAppointment(BookingRecord bookingRecord, int contactID);

        // Cancel appointment
        Task<bool> CancelAppointment(int bookingID);
        Task<List<BookingRecord>> GetAppointmentsByContactID(int contactID);

        // Management
        Task<List<BookingRecord>> GetAppointmentsByDate(DateTime startDate, DateTime endDate);

        Task<BookingRecord> GetAppointmentByID(int bookingID);
    }
}