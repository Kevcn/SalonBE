using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using SalonAPI.Domain;
using SalonAPI.Repository;
using SalonAPI.Services;
using Serilog;
using Xunit;

namespace Tests
{
    public class AppointmentServiceTests
    {
        public AppointmentServiceTests()
        {
            // _appointmentService = new AppointmentService(_appointmentRepoMock.Object, _contactRepoMock.Object);
        }

        private readonly AppointmentService _appointmentService;
        private readonly Mock<IContactRepository> _contactRepoMock = new Mock<IContactRepository>();
        private readonly Mock<IAppointmentRepository> _appointmentRepoMock = new Mock<IAppointmentRepository>();

        [Fact]
        public async Task BookAppointment_ShouldBeSuccessful_ForExistingContact()
        {
            var dummyContact = new Contact
            {
                ID = 10
            };

            var dummyBookingRecord = new BookingRecord
            {
                contact = dummyContact
            };

            _appointmentRepoMock.Setup(x => x.VerifyTimeSlotAvailable(dummyBookingRecord)).ReturnsAsync(true);
            _contactRepoMock.Setup(x => x.CheckDuplicate(dummyBookingRecord.contact))
                .ReturnsAsync(dummyContact);
            _appointmentRepoMock.Setup(x => x.AddAppointment(dummyBookingRecord, dummyContact.ID)).ReturnsAsync(true);

            var response = await _appointmentService.BookAppointment(dummyBookingRecord);

            Assert.True(response);
        }

        [Fact]
        public async Task BookAppointment_ShouldBeSuccessful_ForNewContact()
        {
            var dummyContact = new Contact
            {
                ID = 0
            };

            var dummyAddedContactID = 10;

            var dummyBookingRecord = new BookingRecord
            {
                contact = dummyContact
            };

            _appointmentRepoMock.Setup(x => x.VerifyTimeSlotAvailable(dummyBookingRecord)).ReturnsAsync(true);
            _contactRepoMock.Setup(x => x.CheckDuplicate(dummyBookingRecord.contact))
                .ReturnsAsync(dummyContact);
            _contactRepoMock.Setup(x => x.AddContact(dummyContact)).ReturnsAsync(dummyAddedContactID);
            _appointmentRepoMock.Setup(x => x.AddAppointment(dummyBookingRecord, dummyAddedContactID))
                .ReturnsAsync(true);

            var response = await _appointmentService.BookAppointment(dummyBookingRecord);

            Assert.True(response);
        }

        [Fact]
        public async Task BookAppointment_ShouldReturnFalse_WhenTimeSlotIsTaken()
        {
            var dummyBookingRecord = new BookingRecord();
            _appointmentRepoMock.Setup(x => x.VerifyTimeSlotAvailable(dummyBookingRecord)).ReturnsAsync(false);

            var timeSlotavailability = await _appointmentService.BookAppointment(dummyBookingRecord);

            Assert.False(timeSlotavailability);
        }

        [Fact]
        public async Task CancelAppointment_ShouldReturnFalse_WhenProvidedWithInvalidBookingID()
        {
            var dummyInvalidBookingID = 10;
            _appointmentRepoMock.Setup(x => x.CancelAppointment(dummyInvalidBookingID)).ReturnsAsync(false);

            var response = await _appointmentService.CancelAppointment(dummyInvalidBookingID);

            Assert.False(response);
        }

        [Fact]
        public async Task CancelAppointment_ShouldReturnTrue_WhenProvidedWithValidBookingID()
        {
            var dummyValidBookingID = 10;
            _appointmentRepoMock.Setup(x => x.CancelAppointment(dummyValidBookingID)).ReturnsAsync(true);

            var response = await _appointmentService.CancelAppointment(dummyValidBookingID);

            Assert.True(response);
        }

        [Fact]
        public async Task GetAppointmentsByContact_ShouldReturnListOfBookingRecords_ForContactWithAppointments()
        {
            var dummyContact = new Contact();
            var dummyContactID = 1;

            var dummyBookingRecords = new List<BookingRecord>
            {
                new BookingRecord {ID = 1, TimeSlotID = 1, Date = new DateTime(2020, 8, 10), Description = "Desc1"},
                new BookingRecord {ID = 2, TimeSlotID = 2, Date = new DateTime(2020, 8, 11), Description = "Desc2"},
                new BookingRecord {ID = 3, TimeSlotID = 3, Date = new DateTime(2020, 8, 12), Description = "Desc3"}
            };

            _contactRepoMock.Setup(x => x.GetContactID(dummyContact)).ReturnsAsync(dummyContactID);
            _appointmentRepoMock.Setup(x => x.GetAppointmentsByContactID(dummyContactID))
                .ReturnsAsync(dummyBookingRecords);

            var bookingRecords = await _appointmentService.GetAppointmentsByContact(dummyContact);

            Assert.Equal(dummyBookingRecords.First().ID, bookingRecords.First().ID);
            Assert.Equal(dummyBookingRecords[1].Description, bookingRecords[1].Description);
            Assert.Equal(dummyBookingRecords[2].TimeSlotID, bookingRecords[2].TimeSlotID);
            Assert.Equal(dummyBookingRecords[2].Date, bookingRecords[2].Date);
        }

        [Fact]
        public async Task GetAppointmentsByContact_ShouldReturnNoBookingRecords_ForInvalidContact()
        {
            var dummyContact = new Contact();
            var dummyContactID = 0;

            _contactRepoMock.Setup(x => x.GetContactID(dummyContact)).ReturnsAsync(dummyContactID);

            var bookingRecords = await _appointmentService.GetAppointmentsByContact(dummyContact);

            Assert.Empty(bookingRecords);
        }

        [Fact]
        public async Task GetDayavailability_ShouldHaveavailability_WhenLessThan16Appointments()
        {
            var date = new DateTime(2020, 7, 23);
            var endDate = date.AddDays(14);
            var dummyBookingsRecords = new List<BookingRecord>
            {
                new BookingRecord
                {
                    Date = date,
                    TimeSlotID = 1
                },
                new BookingRecord
                {
                    Date = date,
                    TimeSlotID = 2
                },
                new BookingRecord
                {
                    Date = date,
                    TimeSlotID = 3
                }
            };

            _appointmentRepoMock.Setup(x => x.GetAppointmentsByDay(date, endDate)).ReturnsAsync(dummyBookingsRecords);

            var availablities = await _appointmentService.GetDayavailability(date);

            Assert.True(availablities.First().Available);
        }

        [Fact]
        public async Task GetDayavailability_ShouldHaveNoavailability_WhenThereAre16Appointments()
        {
            var date = new DateTime(2020, 7, 23);
            var endDate = date.AddDays(14);
            var dummyBookingsRecords = new List<BookingRecord>();

            for (var i = 0; i <= 16; i++)
                dummyBookingsRecords.Add(new BookingRecord
                {
                    Date = date,
                    TimeSlotID = i
                });

            _appointmentRepoMock.Setup(x => x.GetAppointmentsByDay(date, endDate)).ReturnsAsync(dummyBookingsRecords);

            var availablities = await _appointmentService.GetDayavailability(date);

            Assert.False(availablities.First().Available);
        }

        [Fact]
        public async Task GetTimeavailability_ShouldHaveCorrectavailability_ForEachTimeSlots()
        {
            var date = new DateTime(2020, 7, 23);
            var dummyBookingsRecords = new List<BookingRecord>
            {
                new BookingRecord {TimeSlotID = 2}
            };

            _appointmentRepoMock.Setup(x => x.GetSingleDayAppointments(date)).ReturnsAsync(dummyBookingsRecords);

            var response = await _appointmentService.GetTimeavailability(date);

            Assert.True(response[0].Available);
            Assert.False(response[1].Available);
            Assert.True(response[2].Available);
        }
    }
}