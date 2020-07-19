using System.Threading.Tasks;
using SalonAPI.Domain;

namespace SalonAPI.Repository
{
    public interface IContactRepository
    {
        Task<int> AddContact(Contact contact);
    }
}