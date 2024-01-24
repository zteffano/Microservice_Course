﻿using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;

namespace CommandsService.Data
{
	public static class PrepDb
	{
		public static void PrepPopulation(IApplicationBuilder applicationBuilder)
		{
			using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
			{
				var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();

				var platforms = grpcClient.ReturnAllPlatforms();
				
				SeedData(serviceScope.ServiceProvider.GetService<ICommandRepo>(), platforms);
			}
		}

		private static void SeedData(ICommandRepo repo, IEnumerable<Platform> platforms) 
		{
            Console.WriteLine("--> Seeding new platforms with grpc...");

			foreach (Platform platform in platforms) 
			{
				if(!repo.ExternalPlatformExist(platform.ExternalId))
				{

					repo.CreatePlatform(platform);

				}
				repo.SaveChanges();
			}
        }
	}
}
