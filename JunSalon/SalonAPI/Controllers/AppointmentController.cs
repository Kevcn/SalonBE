using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.V1;
using Contracts.V1.Requests;
using Contracts.V1.Responses;
using Microsoft.AspNetCore.Mvc;
using SalonAPI.Domain;
using SalonAPI.Services;

namespace SalonAPI.Controllers
{
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentervice;
        private readonly IMapper _mapper;

        public AppointmentController(IMapper mapper, IAppointmentService appointmentService)
        {
            _mapper = mapper;
            _appointmentervice = appointmentService;
        }

        [HttpGet(ApiRoutes.Appointment.GetDayAvailablity)]
        public async Task<IActionResult> GetDayAvailablity([FromRoute] DateTime date)
        {
            var availablity = await _appointmentervice.GetDayAvailablity(date);
            return Ok(_mapper.Map<List<DayAvailabilityResponse>>(availablity));
        }
        
        [HttpGet(ApiRoutes.Appointment.GetTimeAvailablity)]
        public async Task<IActionResult> GetTimeAvailablity([FromRoute] DateTime date)
        {
            var availablity = await _appointmentervice.GetTimeAvailablity(date);
            return Ok(_mapper.Map<List<TimeAvailabilityResponse>>(availablity));
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
            
            var booked = await _appointmentervice.BookAppointment(bookingRecord);

            if (!booked)
            {
                return BadRequest(new {error = "Book appointment failed"});
            }
            
            return Ok(_mapper.Map<BookingResponse>(bookingRecord));
        }
        
        [HttpPost(ApiRoutes.Appointment.Cancel)]
        public async Task<IActionResult> Cancel([FromBody] int bookingID)
        {
            var cancelled = await _appointmentervice.CancelAppointment(bookingID);

            if (!cancelled)
            {
                return BadRequest(new {error = "Cancel appointment failed"});
            }

            return Ok();
        }

        [HttpPost(ApiRoutes.Appointment.GetAppointment)]
        public async Task<IActionResult> GetAppointment([FromBody] Contact contact)
        {
            var bookingRecords = await _appointmentervice.GetAppointmentsByContact(contact);
            //TODO: map to response object
            
            return Ok(_mapper.Map<List<BookingResponse>>(bookingRecords));
        }
        
        [HttpGet(ApiRoutes.Appointment.ViewBooking)] // Managers view
        public async Task<IActionResult> GetAll(string dateFrom, string dateTo)
        {
            // Defaults to the coming 2 weeks
            return null;
        }
    }
}