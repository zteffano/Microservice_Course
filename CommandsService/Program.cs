
using CommandsService.AsyncDataServices;
using CommandsService.Data;
using CommandsService.EventProcessing;
using CommandsService.SyncDataServices.Grpc;
using Microsoft.EntityFrameworkCore;

namespace CommandsService
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			builder.Services.AddHostedService<MessageBusSubscriber>();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
			builder.Services.AddScoped<ICommandRepo, CommandRepo>();
			builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
			builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
			builder.Services.AddScoped<IPlatformDataClient, PlatformDataClient>();
			

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			//app.UseHttpsRedirection(); // Bruger ikke https i dette projekt og for at slippe for fejlmeddelelse i browseren

			app.UseAuthorization();


			app.MapControllers();
			PrepDb.PrepPopulation(app);

			app.Run();
		}
	}
}
