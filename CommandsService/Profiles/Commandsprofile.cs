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

			CreateMap<PlatformID, PlatformReadDto>();
			CreateMap<Command, CommandReadDto>();
			CreateMap<CommandCreateDto, Command>();
		}
	}
}
