using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SalonAPI.Domain;
using SalonAPI.Repository;

namespace SalonAPI.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IContactRepository _contactRepository;
        private readonly ILogger<AppointmentService> _logger; 

        public AppointmentService(IAppointmentRepository appointmentRepository, IContactRepository contactRepository, ILogger<AppointmentService> logger)
        {
            _appointmentRepository = appointmentRepository;
            _contactRepository = contactRepository;
            _logger = logger;
        }

        public async Task<List<DayAvailability>> GetDayavailability(DateTime date)
        {
            var availablities = new List<DayAvailability>();
            var endDate = date.AddDays(14);

            var bookingRecords = await _appointmentRepository.GetAppointmentsByDay(date, endDate);

            for (var i = 0; i < 14; i++)
            {
                var currentDate = date.AddDays(i);

                var availability = new DayAvailability
                {
                    date = currentDate,
                    Available = bookingRecords.Count(x => x.Date == currentDate) < 16
                };

                availablities.Add(availability);
            }

            return availablities;
        }

        public async Task<List<TimeAvailability>> GetTimeavailability(DateTime date)
        {
            var availablities = new List<TimeAvailability>();
            var numberOfTimeSlots = 16;

            var bookingRecords = await _appointmentRepository.GetSingleDayAppointments(date);

            for (var i = 1; i <= numberOfTimeSlots; i++)
            {
                var availability = new TimeAvailability
                {
                    TimeSlotID = i,
                    Available = bookingRecords.All(x => x.TimeSlotID != i)
                };

                availablities.Add(availability);
            }

            return availablities;
        }

        public async Task<bool> BookAppointment(BookingRecord bookingRecord)
        {
            var removeTimePortion = new TimeSpan(0, 0, 0);
            bookingRecord.Date = bookingRecord.Date.Date + removeTimePortion;

            var timeSlotavailability = await _appointmentRepository.VerifyTimeSlotAvailable(bookingRecord);

            if (!timeSlotavailability)
                // TODO: log time slot unavailable
                return false;

            var existingContact = await _contactRepository.CheckDuplicate(bookingRecord.contact);

            var contactID = existingContact.ID != 0
                ? existingContact.ID
                : await _contactRepository.AddContact(bookingRecord.contact);

            var added = await _appointmentRepository.AddAppointment(bookingRecord, contactID);
            // TODO: log added successful
            return added;
        }

        public async Task<BookingRecord> GetAppointment(int bookingID)
        {
            return await _appointmentRepository.GetAppointmentByID(bookingID);
        }

        public async Task<bool> CancelAppointment(int bookingID)
        {
            return await _appointmentRepository.CancelAppointment(bookingID);
        }

        public async Task<List<BookingRecord>> GetAppointmentsByContact(Contact contactDetails)
        {
            var contactID = await _contactRepository.GetContactID(contactDetails);

            if (contactID == 0) return new List<BookingRecord>();

            return await _appointmentRepository.GetAppointmentsByContactID(contactID);
        }

        public async Task<List<BookingRecord>> GetAppointmentByDate(DateTime startDate, DateTime endDate)
        {
            var orderedRecords = new List<BookingRecord>();

            var records = await _appointmentRepository.GetAppointmentsByDate(startDate, endDate);
            var groupByDay = records.GroupBy(x => x.Date);
            foreach (var date in groupByDay)
            {
                var orderedDay = date.OrderBy(x => x.TimeSlotID);

                orderedRecords.AddRange(orderedDay.ToList());
            }

            return orderedRecords;
        }
    }
}