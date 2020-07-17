using Contracts.V1.Requests;
using Contracts.V1.Responses;
using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Financial_API.Sdk
{
    [Headers("Authorization: Bearer")]
    public interface IFinancialAPI
    {
        [Get("/api/v1/stocks")]
        Task<ApiResponse<List<StockResponse>>> GetAllAsync();

        [Get("/api/v1/stocks/{stockId}")]
        Task<ApiResponse<StockResponse>> GetAsync(Guid stockId);
    }
}
