using AutoMapper;
using Contracts.V1.Responses;
using SalonAPI.Domain;

namespace SalonAPI.MappingProfiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            // Maps domain to response
            CreateMap<Stock, StockResponse>();
            CreateMap<TimeAvailability, TimeAvailabilityResponse>();
            CreateMap<DayAvailability, DayAvailabilityResponse>();
            CreateMap<BookingRecord, BookingResponse>();
        }
    }
}
