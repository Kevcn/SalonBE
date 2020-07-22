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
        public async Task<IActionResult> Book([FromBody] BookingRecord bookingRecord)
        {
            // Takes Name, phone and date, timeslot ID
            
            // TODO: create bookingrecordRequest object, new bookingrecord from that
            // var bookingRecord = new BookingRecord
            // {
            //     contact = bookingRequest
            // };
            
            var booked = await _appointmentervice.BookAppointment(bookingRecord);

            if (!booked)
            {
                // TODO: what to return for failed insert?
                return NotFound();
            }
            
            return Ok(_mapper.Map<BookingResponse>(bookingRecord));
        }
        
        [HttpPost(ApiRoutes.Appointment.Cancel)]
        public async Task<IActionResult> Cancel([FromBody] BookingRecord bookingRequest)
        {
            // Provide phone number and date
            
            
            var bookingRecord = new BookingRecord();
            
            var cancelled = await _appointmentervice.CancelAppointment(bookingRecord);

            if (!cancelled)
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpPost(ApiRoutes.Appointment.GetAppointment)]
        public async Task<IActionResult> GetAppointment([FromBody] Contact contact)
        {
            // Return a list of bookingRecords
            return NotFound();
        }
        
        [HttpGet(ApiRoutes.Appointment.ViewBooking)] // Managers view
        public async Task<IActionResult> GetAll(string dateFrom, string dateTo)
        {
            // Defaults to the coming 2 weeks
            return null;
        }
    }
}