using System.Linq;
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
            CreateMap<BookingRecord, BookingResponse>()
                .ForPath(dest => dest.contact.Name, opt => 
                    opt.MapFrom(src => src.contact.Name))
                .ForPath(dest => dest.contact.Phone, opt =>
                    opt.MapFrom(src => src.contact.Phone))
                .ForPath(dest => dest.contact.Email, opt => 
                    opt.MapFrom(src => src.contact.Email));
        }
    }
}
