using System;
using System.Collections.Generic;
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

        public async Task<List<DayAvailability>> GetDayAvailablity(string date)
        {
            // TODO: add day range?
            
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
            // insert into contact - // TODO: check for duplicates before insert
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