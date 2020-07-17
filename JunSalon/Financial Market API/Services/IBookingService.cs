using System.Threading.Tasks;
using Financial_Market_API.Domain;

namespace Financial_Market_API.Services
{
    public interface IBookingService
    {
        Task<Stock> GetAvailablities();
    }
}