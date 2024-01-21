using PlatformService.Model;

namespace PlatformService.Data
{
	/* 
	 Prepper og Seeder data til databasen
	 */
	 
	public static class PrepDb
	{
		public static void PrepPopulation(IApplicationBuilder app)
		{

			using (var ServiceScope = app.ApplicationServices.CreateScope())
			{
				SeedData(ServiceScope.ServiceProvider.GetService<AppDbContext>());
			}

		}
		

		private static void SeedData(AppDbContext context)
		{
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
