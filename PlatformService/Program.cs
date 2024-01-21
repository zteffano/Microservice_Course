
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
			builder.Services.AddDbContext<AppDbContext>(opt => 
				opt.UseInMemoryDatabase("InMem"));
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
			builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
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

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();
			PrepDb.PrepPopulation(app);

			app.Run();
		}
	}
}
