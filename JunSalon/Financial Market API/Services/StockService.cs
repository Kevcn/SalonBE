using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Financial_Market_API.Data;
using Financial_Market_API.Domain;
using Microsoft.EntityFrameworkCore;

namespace Financial_Market_API.Services
{
    public class StockService : IStockService
    {
        private readonly ApplicationDbContext _dbContext;

        public StockService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Stock>> GetStocksAsync()
        {
            return await _dbContext.Stocks.ToListAsync();
        }

        public async Task<Stock> GetStockByIdAsync(Guid stockId)
        {
            return await _dbContext.Stocks.SingleOrDefaultAsync(s => s.Id == stockId);
        }

        public async Task<bool> CreateStockAsync(Stock stock)
        {
            await _dbContext.AddAsync(stock);
            var created = await _dbContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> UpdatePostAsync(Stock stockToUpdate)
        {
            _dbContext.Stocks.Update(stockToUpdate);
            // This returns the number of records affected by the update command
            var updated = await _dbContext.SaveChangesAsync();
            // < 0 indicates there is at least a record been updated, hence true
            return updated > 0;
            
        }
        public async Task<bool> DeleteStockAsync(Guid stockId)
        {
            var stock = await GetStockByIdAsync(stockId);
            _dbContext.Stocks.Remove(stock);
            var deleted = await _dbContext.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<bool> UserOwnsStockAsync(Guid stockId, string userId)
        {
            // AsNoTracking() - not tracked by SaveChanges()
            var stock = await _dbContext.Stocks.AsNoTracking().SingleOrDefaultAsync(x => x.Id == stockId);

            // Determine if user owns the stock
            if (stock == null || stock.UserId != userId)
            {
                return false;
            }

            return true;
        }
    }
}
