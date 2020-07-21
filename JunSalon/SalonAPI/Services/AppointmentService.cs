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
            
            for (int i = 0; i < numberOfTimeSlots; i++)
            {
                var availablity = new TimeAvailability
                {
                    TimeSlotID = i,
                    Available = bookingRecords.Any(x => x.TimeSlotID == i) // TODO: THIS LOGIC IS WRONG
                };
                
                availablities.Add(availablity);
            }
            
            return availablities;
        }

        public async Task<bool> BookAppointment(BookingRecord bookingRecord)
        {
            // insert into contact - // TODO: check for duplicates before insert, use DB trigger? return -1 if duplicate
            var contactID =  await _contactRepository.AddContact(bookingRecord.contact);
            
            // insert into bookingRecord
            var added = await _appointmentRepository.AddAppointment(bookingRecord, contactID);

            return added;
        }

        public async Task<bool> CancelAppointment(BookingRecord bookingRecord)
        {
            // delete from bookingRecord
            
            // check date is in the future
            
            throw new System.NotImplementedException();
        }

        public async Task<BookingRecord> GetRecord(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}