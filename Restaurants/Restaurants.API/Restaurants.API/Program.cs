using Microsoft.OpenApi.Writers;
using Restaurants.Infrastructure.Extensions;
using Restaurants.Application.Extenstions;
using Restaurants.Infrastructure.Seeders;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Restaurants.API.Middlewares;
using Restaurants.Domain.Entities;
var builder = WebApplication.CreateBuilder(args);
//new CompactJsonFormatter();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<RequestTimeLoggingMiddleware>();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Host.UseSerilog((context, configuration) =>
{   
    configuration
    .ReadFrom.Configuration(context.Configuration); 
    //.MinimumLevel.Override("Microsoft",LogEventLevel.Warning)
    //.MinimumLevel.Override("Microsoft.EntityFrameworkCore",LogEventLevel.Information)
    //.WriteTo.File("Logs/Restaurant-Api-.log",rollingInterval:RollingInterval.Day,rollOnFileSizeLimit:true)
    //.WriteTo.Console(outputTemplate: "[{Timestamp:dd-MM HH:mm:ss} {Level:u3}] |{SourceContext}| {NewLine} {Message:lj}{NewLine}{Exception}");
});
var app = builder.Build();
var scope=app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();
await seeder.Seed();
// Configure the HTTP request pipeline.
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestTimeLoggingMiddleware>();
app.UseSerilogRequestLogging();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.MapIdentityApi<User>();
app.UseAuthorization();

app.MapControllers();

app.Run();
