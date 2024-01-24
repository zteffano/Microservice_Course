using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;

namespace CommandsService.Profiles
{
	public class Commandsprofile : Profile
	{

		public Commandsprofile() 
		{
			//Source -> Target

			CreateMap<Platform, PlatformReadDto>();
			CreateMap<Command, CommandReadDto>();
			CreateMap<CommandCreateDto, Command>();
			// Mapper PlatformPublishedDto Id til Platform External Id 
			CreateMap<PlatformPublishedDto, Platform>()
				.ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

			CreateMap<GrpcPlatformModel, Platform>()
				.ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.PlatformId))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)) //for en sikkerhedsskyld pga. vi bruger grpc
				.ForMember(dest => dest.Commands, opt => opt.Ignore()); 


		}
	}
}
