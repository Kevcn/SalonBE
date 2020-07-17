using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalonAPI.Domain;

namespace SalonAPI.Services
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
