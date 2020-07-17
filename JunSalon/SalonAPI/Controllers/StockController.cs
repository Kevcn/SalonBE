
using AutoMapper;
using Contracts.V1;
using Contracts.V1.Requests;
using Contracts.V1.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SalonAPI.Domain;
using SalonAPI.Services;

namespace SalonAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;

        // Auto Mapper DI
        private readonly IMapper _mapper;

        public StockController(IStockService stockService, IMapper mapper)
        {
            _stockService = stockService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns a list of Stocks
        /// </summary>
        /// <response code="200">Creates a stock in the system</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet(ApiRoutes.Stock.GetAll)]
        [Authorize(Policy = "AccessToStocks")] // This claim is given upon registration
        public async Task<IActionResult> GetAll()
        {
            var stocks = await _stockService.GetStocksAsync();
            return Ok(_mapper.Map<List<StockResponse>>(stocks));
        }

        [Authorize(Roles = "Admin")] // This claim is given upon registration
        [Authorize(Policy = "GmailUser")] 
        [HttpPost(ApiRoutes.Stock.Create)]
        public async Task<IActionResult> Create([FromBody] CreateStockRequest stockRequest)
        {
            // Seperate contract and domain object!
            Stock stock = new Stock
            {
                Id = Guid.NewGuid(),
                Name = stockRequest.Name,
                Company = stockRequest.Company,
                UserId = HttpContext.GetUserId()
            };

            // Save to DB
            await _stockService.CreateStockAsync(stock);

            // Construct response object from domain object
            var response = _mapper.Map<StockResponse>(stock);

            // The location url will be listed in header
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            string locationUrl = baseUrl + "/" + ApiRoutes.Stock.Get.Replace("{stockId}", stock.Id.ToString());
            return Created(locationUrl, response);
        }

        [HttpGet(ApiRoutes.Stock.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid stockId)
        {
            var stock = await _stockService.GetStockByIdAsync(stockId);

            if (stock == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<StockResponse>(stock);

            return Ok(response);
        }

        [HttpPut(ApiRoutes.Stock.Update)] // Full updates the resource(only Id remain the same. Patch on the other hand, only updates one aspect of the resource)
        public async Task<IActionResult> Update([FromRoute] Guid stockId, [FromBody] UpdateStockRequest stockToUpdate)
        {
            // Check for ownership
            var ownership = await _stockService.UserOwnsStockAsync(stockId, HttpContext.GetUserId());

            if (!ownership)
            {
                return BadRequest(new {error = "no ownership"});
            }

            // Get stock
            var stock = await _stockService.GetStockByIdAsync(stockId);

            stock.Name = stockToUpdate.Name;
            stock.Company = stockToUpdate.Company;

            var updated = await _stockService.UpdatePostAsync(stock);

            if (!updated)
            {
                return NotFound();
            }

            var response = _mapper.Map<StockResponse>(stock);

            return Ok(response);

        }

        [HttpDelete(ApiRoutes.Stock.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid stockId)
        {
            // Check for ownership
            var ownership = await _stockService.UserOwnsStockAsync(stockId, HttpContext.GetUserId());

            if (!ownership)
            {
                return BadRequest(new { error = "no ownership" });
            }

            // As per REST standard, we can either return the resource or return 204 to indicate that the resource has been deleted
            var deleted = await _stockService.DeleteStockAsync(stockId);

            if (deleted)
            {
                // Status code of 204
                return NoContent();
            }

            // 404
            return NotFound();

        }

    }
}
