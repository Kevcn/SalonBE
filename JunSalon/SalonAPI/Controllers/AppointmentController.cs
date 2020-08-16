using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SalonAPI.Contracts.V1;
using SalonAPI.Contracts.V1.Requests;
using SalonAPI.Contracts.V1.Responses;
using SalonAPI.Domain;
using SalonAPI.Services;

namespace SalonAPI.Controllers
{
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IMapper _mapper;

        public AppointmentController(IMapper mapper, IAppointmentService appointmentService)
        {
            _mapper = mapper;
            _appointmentService = appointmentService;
        }

        [HttpGet(ApiRoutes.Appointment.GetDayavailability)]
        public async Task<IActionResult> GetDayavailability([FromRoute] DateTime date)
        {
            var availability = await _appointmentService.GetDayavailability(date);
            return Ok(_mapper.Map<List<DayAvailabilityResponse>>(availability));
        }
        
        [HttpGet(ApiRoutes.Appointment.GetTimeavailability)]
        public async Task<IActionResult> GetTimeavailability([FromRoute] DateTime date)
        {
            var availability = await _appointmentService.GetTimeavailability(date);
            return Ok(_mapper.Map<List<TimeAvailabilityResponse>>(availability));
        }
        
        [HttpPost(ApiRoutes.Appointment.Book)]
        public async Task<IActionResult> Book([FromBody] BookingRecordRequest bookingRecordRequest)
        {
            var bookingRecord = new BookingRecord
            {
                contact = new Contact
                {
                    Name = bookingRecordRequest.contact.Name,
                    Phone = bookingRecordRequest.contact.Phone,
                    Email = bookingRecordRequest.contact.Email
                },
                TimeSlotID = bookingRecordRequest.TimeSlotID,
                Date = bookingRecordRequest.Date,
                Description = bookingRecordRequest.Description
            };
            
            var booked = await _appointmentService.BookAppointment(bookingRecord);

            if (!booked)
            {
                return BadRequest(new {error = "Book appointment failed"});
            }
            
            return Ok(_mapper.Map<BookingResponse>(bookingRecord));
        }
        
        [HttpGet(ApiRoutes.Appointment.Get)]
        public async Task<IActionResult> Get([FromRoute] int bookingID)
        {
            var bookingRecord = await _appointmentService.GetAppointment(bookingID);

            if (bookingRecord.ID == 0)
            {
                return NotFound();
            }
            
            return Ok(_mapper.Map<BookingResponse>(bookingRecord));
        }

        
        [HttpPost(ApiRoutes.Appointment.Cancel)]
        public async Task<IActionResult> Cancel([FromBody] int bookingID)
        {
            var cancelled = await _appointmentService.CancelAppointment(bookingID);

            if (!cancelled)
            {
                return BadRequest(new {error = "Cancel appointment failed"});
            }

            return Ok();
        }

        [HttpPost(ApiRoutes.Appointment.GetByContact)]
        public async Task<IActionResult> GetAppointment([FromBody] ContactRequest contactRequest)
        {
            var contact = new Contact
            {
                Name = contactRequest.Name,
                Phone = contactRequest.Phone,
                Email = contactRequest.Email
            };
            
            var bookingRecords = await _appointmentService.GetAppointmentsByContact(contact);
            
            return Ok(_mapper.Map<List<BookingResponse>>(bookingRecords));
        }
        
        [HttpGet(ApiRoutes.Appointment.GetByDate)] // Managers view
        public async Task<IActionResult> GetAll(DateTime dateFrom, DateTime dateTo)
        {
            var bookingRecords = await _appointmentService.GetAppointmentByDate(dateFrom, dateTo);
            
            return Ok(_mapper.Map<List<BookingResponse>>(bookingRecords));
        }
    }
}