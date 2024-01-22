using Microsoft.EntityFrameworkCore;
using PlatformService.Model;

namespace PlatformService.Data
{
	/* 
	 Prepper og Seeder data til databasen
	 */
	 
	public static class PrepDb
	{
		public static void PrepPopulation(IApplicationBuilder app, bool isProd)
		{

			using (var ServiceScope = app.ApplicationServices.CreateScope())
			{
				SeedData(ServiceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
			}

		}
		

		private static void SeedData(AppDbContext context, bool isProd)
		{

			if(isProd)
			{
				Console.WriteLine("--> Attempting to apply migrations...");
				try
				{
					context.Database.Migrate();
				}
				catch (Exception ex)
				{
					Console.WriteLine($"--> Could not run migrations: {ex.Message}");
				}
			}

			if(!context.Platforms.Any())
			{
				Console.WriteLine("--> Seeding Data...");

				context.Platforms.AddRange(
					new Platform() { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free" },
					new Platform() { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
					new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
				);
				context.SaveChanges();
			}
			else
			{
				Console.WriteLine("--> We already have data");
			}
		}
	}
}
