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

            // order and group by date

            var days = bookingRecords.GroupBy(x => x.Date).ToList();
            
            // List<List<bookingRecords>> days = 
            // if number of bookings is less than 14, should 
            
            foreach (var booking in bookingRecords)
            {
                
            }
            
            // Query booking record on date + 14, if not all time slots are taken for a perticular day, return true
            
            
            
            throw new System.NotImplementedException();
        }

        public async Task<List<TimeAvailability>> GetTimeAvailablity(DateTime date)
        {
            // query where date = date, get a list of bookingRecords, turn that into a list timeAvailable
            
            throw new System.NotImplementedException();
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