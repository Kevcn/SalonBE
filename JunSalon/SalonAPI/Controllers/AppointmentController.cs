using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.V1;
using Contracts.V1.Responses;
using Microsoft.AspNetCore.Mvc;
using SalonAPI.Domain;
using SalonAPI.Services;

namespace SalonAPI.Controllers
{
    public class AppointmentController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;

        public AppointmentController(IMapper mapper, IBookingService bookingService)
        {
            _mapper = mapper;
            _bookingService = bookingService;
        }

        [HttpGet(ApiRoutes.Appointment.GetDayAvailablity)]
        public async Task<IActionResult> GetDayAvailablity([FromRoute] string date)
        {
            var availablity = await _bookingService.GetDayAvailablity(date);
            return Ok(_mapper.Map<List<DayAvailabilityResponse>>(availablity));
        }
        
        [HttpGet(ApiRoutes.Appointment.GetTimeAvailablity)]
        public async Task<IActionResult> GetTimeAvailablity([FromRoute] string date)
        {
            var availablity = await _bookingService.GetTimeAvailablity(date);
            return Ok(_mapper.Map<List<TimeAvailabilityResponse>>(availablity));
        }
        
        [HttpGet(ApiRoutes.Appointment.Book)]
        public async Task<IActionResult> Book([FromBody] BookingRecord bookingRequest)
        {
            // TODO: create bookingrecordRequest object, new bookingrecord from that 
            
            var bookingRecord = new BookingRecord();
            
            var booked = await _bookingService.BookAppointment(bookingRecord);

            if (!booked)
            {
                // TODO: what to return for failed insert?
                return NotFound();
            }
            
            return Ok(_mapper.Map<BookingResponse>(bookingRecord));
        }
        
        [HttpGet(ApiRoutes.Appointment.Cancel)]
        public async Task<IActionResult> Cancel([FromBody] BookingRecord bookingRequest)
        {
            var bookingRecord = new BookingRecord();
            
            var cancelled = await _bookingService.CancelAppointment(bookingRecord);

            if (!cancelled)
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpGet(ApiRoutes.Appointment.ViewBookings)] // Managers view
        public async Task<IActionResult> GetAll(string dateFrom, string dateTo)
        {
            // Defaults to the coming 2 weeks
            return null;
        }
    }
}