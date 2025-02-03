using SparkApi.Models.DbModels;
using AutoMapper;
using SparkApi.Models;
using SparkApi.Models.DTOs.ResponseDTO;

namespace SparkApi.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CsvModel, Event>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Id, opt => opt.Ignore()); //Db handles the Id

            CreateMap<Event, EventDto>();

            // Response - Map db model to DTO
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.EventsDtos, opt => opt.MapFrom(src => src.Events));
        }
    }
}
