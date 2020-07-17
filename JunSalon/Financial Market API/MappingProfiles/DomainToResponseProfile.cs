using AutoMapper;
using Contracts.V1.Responses;
using Financial_Market_API.Domain;

namespace Financial_Market_API.MappingProfiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            // Maps domain to response
            CreateMap<Stock, StockResponse>();
        }
    }
}
