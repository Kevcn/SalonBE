using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.V1.Responses;
using Financial_Market_API.Domain;
using Financial_Market_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Financial_Market_API.Controllers
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

        public async Task<IActionResult> GetAll()
        {
            var bookingRecords = await _stockService.GetStocksAsync();
            return Ok(_mapper.Map<List<StockResponse>>(bookingRecords));
        }
    }
}