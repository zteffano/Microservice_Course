using AutoMapper;
using PlatformService.Dtos;
using PlatformService.Model;

namespace PlatformService.Profiles
{
	public class PlatformProfile : Profile
	{
        public PlatformProfile()
        {
            // Source -> Target
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformCreateDto, Platform>();
            CreateMap<PlatformReadDto, PlatformPublishedDto>();
            CreateMap<Platform, GrpcPlatformModel>()
                .ForMember(dest => dest.PlatformId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)) // Burde ikke være nødvendig, men han har medtaget dem i video for en sikkerhedsskyld
                .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.Publisher)); // Burde ikke være nødvendig, men han har medtaget dem i video for en sikkerhedsskyld

		}
    }
}
