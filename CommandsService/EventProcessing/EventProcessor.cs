﻿using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using System.Text.Json;

namespace CommandsService.EventProcessing
{
	public class EventProcessor : IEventProcessor
	{
		private readonly IServiceScopeFactory _scopeFactory;
		private readonly IMapper _mapper;

		public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
			_mapper = mapper;
        }

        public void ProcessEvent(string message)
		{
			var eventType = DetermineEvent(message);

			switch (eventType)
			{
				case EventType.PlatformPublished:
					AddPlatform(message);
					break;
				default:
					break;
			}
		}

		private EventType DetermineEvent(string notificationMessage)
		{
            Console.WriteLine("--> Determining Event");

			var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

			switch (eventType.Event)
			{
				case "Platform_Published":
                    Console.WriteLine("--> Platform Published Event Detected");
					return EventType.PlatformPublished;

				default:
                    Console.WriteLine("X-> Could not determine event type");
					return EventType.Undetermined;
            }
        }

		private void AddPlatform(string platformPublishedMessage)
		{
			using (var scope = _scopeFactory.CreateScope())
			{
				var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

				var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);

				try
				{
					var plat = _mapper.Map<Platform>(platformPublishedDto);
					if(!repo.ExternalPlatformExist(plat.ExternalId))
					{
						repo.CreatePlatform(plat);
						repo.SaveChanges();
						Console.WriteLine("--> Platform added!");
					}
					else
					{
                        Console.WriteLine("I-> Platform already exists");
                    }
				}
				catch (Exception ex)
				{
                    Console.WriteLine($"X-> Could not add Platform to DB {ex.Message}");
                }
			}
		}
	}

	enum EventType
	{
		PlatformPublished,
		Undetermined
	}
}
