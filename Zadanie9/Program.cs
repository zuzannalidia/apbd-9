using Microsoft.EntityFrameworkCore;
using Zadanie9.Data;
using Zadanie9.Services.Implementations;
using Zadanie9.Services.Interfaces;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddControllers();
        builder.Services.AddScoped<IClientService, ClientService>();
        builder.Services.AddScoped<ITripService, TripService>();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<ITripService, TripService>();
        builder.Services.AddDbContext<MasterContext>(
            options => options.UseSqlServer("Name=ConnectionStrings:Default"));
        
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
        app.Run();
        
    }
}