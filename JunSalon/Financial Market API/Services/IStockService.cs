using Financial_Market_API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Financial_Market_API.Services
{
    public interface IStockService
    {
        Task<List<Stock>> GetStocksAsync();
        
        Task<Stock> GetStockByIdAsync(Guid stockId);

        Task<bool> CreateStockAsync(Stock stock);

        Task<bool> UpdatePostAsync(Stock stockToUpdate);

        Task<bool> DeleteStockAsync(Guid stockId);


        Task<bool> UserOwnsStockAsync(Guid stockId, string userId);
    }
}
