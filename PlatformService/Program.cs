
using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

namespace PlatformService
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
			builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

			if (builder.Environment.IsProduction())
			{
				Console.WriteLine("--> Using SQL Server DB [Production]");
				builder.Services.AddDbContext<AppDbContext>(opt =>
									opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn")));
			}
			else
			{
				Console.WriteLine("--> Using InMem DB [Development]");
				builder.Services.AddDbContext<AppDbContext>(opt =>
													opt.UseInMemoryDatabase("InMem"));
			}
	

			// automappper
			builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

			Console.WriteLine($"--> CommandService Endpoint {builder.Configuration["CommandService"]}");

			var app = builder.Build();
			
			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
			
				app.UseSwagger();
				app.UseSwaggerUI();
			}
	

			//app.UseHttpsRedirection();  // Bruger ikke https i dette projekt og for at slippe for fejlmeddelelse i browseren

			app.UseAuthorization();


			app.MapControllers();
			PrepDb.PrepPopulation(app, app.Environment.IsProduction());

			app.Run();
		}
	}
}
