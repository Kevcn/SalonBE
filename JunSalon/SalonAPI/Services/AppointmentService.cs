using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalonAPI.Domain;
using SalonAPI.Repository;

namespace SalonAPI.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IContactRepository _contactRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository, IContactRepository contactRepository)
        {
            _appointmentRepository = appointmentRepository;
            _contactRepository = contactRepository;
        }

        public async Task<List<DayAvailability>> GetDayAvailablity(DateTime date)
        {
            var availablities = new List<DayAvailability>();
            
            var endDate = date.AddDays(14);

            var bookingRecords = await _appointmentRepository.GetAppointmentsByDay(date, endDate);

            // add 14 days into the list, for each day, search the number of record, return true for less than 16 records
            for (int i = 0; i < 14; i++)
            {
                var currentDate = date.AddDays(i);
                
                var availablity = new DayAvailability
                {
                    date = currentDate,
                    Available = bookingRecords.Count(x => x.Date == currentDate) < 16
                };
                
                availablities.Add(availablity);
            }

            return availablities;
        }

        public async Task<List<TimeAvailability>> GetTimeAvailablity(DateTime date)
        {
            // query where date = date, get a list of bookingRecords, turn that into a list timeAvailable

            var numberOfTimeSlots = 16;
            
            var availablities = new List<TimeAvailability>();

            var bookingRecords = await _appointmentRepository.GetSingleDayAppointments(date);

            // if timeslot exists, return false
            
            for (int i = 1; i <= numberOfTimeSlots; i++)
            {
                var availablity = new TimeAvailability
                {
                    TimeSlotID = i,
                    Available = bookingRecords.All(x => x.TimeSlotID != i)
                };
                
                availablities.Add(availablity);
            }
            
            return availablities;
        }

        public async Task<bool> BookAppointment(BookingRecord bookingRecord)
        {
            var timeSlotAvailablity = await _appointmentRepository.VerifyTimeSlotAvailable(bookingRecord);

            if (!timeSlotAvailablity)
            {
                // TODO: log time slot unavailable
                return false;
            }

            var existingContact = await _contactRepository.CheckDuplicate(bookingRecord.contact);

            var contactID = existingContact.ID != 0 ? existingContact.ID : await _contactRepository.AddContact(bookingRecord.contact);
            
            var added = await _appointmentRepository.AddAppointment(bookingRecord, contactID);
            // TODO: log added successful
            return added;
        }

        public async Task<bool> CancelAppointment(int bookingID)
        {
            return await _appointmentRepository.CancelAppointment(bookingID);
        }

        public async Task<List<BookingRecord>> GetAppointmentsByContact(Contact contactDetails)
        {
            var contactID = await _contactRepository.GetContactID(contactDetails);

            if (contactID == 0)
            {
                return new List<BookingRecord>();
            }

            return await _appointmentRepository.GetAppointmentsByContactID(contactID);
        }

        // TODO: For management
        public async Task<List<BookingRecord>> GetAppointmentByDate(DateTime startDate, DateTime endDate)
        {
            return await _appointmentRepository.GetAppointmentsByDate(startDate, endDate);
        }
    }
}