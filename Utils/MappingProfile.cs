using SparkApi.Models.DbModels;
using AutoMapper;
using SparkApi.Models.DTOs;

namespace SparkApi.Utils
{
    // After moving the filtering/aggregation to the backend I moved away
    // from most of the mapping here, but I'll leave it anyway
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Event, EventDto>()
                            .ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.Id));


            // Response - Map db model to DTO
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.EventDtos, opt => opt.MapFrom(src => src.Events));
        }
    }
}
