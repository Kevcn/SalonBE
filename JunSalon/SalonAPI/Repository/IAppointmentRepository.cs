using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SalonAPI.Domain;

namespace SalonAPI.Repository
{
    public interface IAppointmentRepository
    {
        // Check availablity 
        Task<List<BookingRecord>> GetAppointmentsByDay(DateTime startDate, DateTime endDate);
        Task<List<BookingRecord>> GetSingleDayAppointments(DateTime date);

        // Book appointment
        Task<bool> VerifyTimeSlotAvailable(BookingRecord bookingRecord);
        Task<bool> AddAppointment(BookingRecord bookingRecord, int contactID);

        // Cancel appointment
        Task<bool> CancelAppointment(int bookingID);
        Task<int> GetContactID(Contact contact);
        Task<List<BookingRecord>> GetAppointments(int contactID);
        
        // Management
        Task<BookingRecord> GetRecord(DateTime startDate, DateTime endDate);
    }
    
}