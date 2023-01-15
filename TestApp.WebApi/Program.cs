using Microsoft.OpenApi.Models;
using Serilog;
using TestApp.Core;
using TestApp.Core.Configuration;
using TestApp.WebApi.Configuration;
using TestApp.WebApi.Handlers;
using TestApp.WebApi.Handlers.Models;
using TestApp.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
var logger = new LoggerConfiguration();
services.AddAuthorization();
services.AddAuthentication();

builder.Host.UseSerilog((hostContext, services, configuration) =>
{
    configuration
    .WriteTo.File("logs.txt")
    .WriteTo.Console();
});
services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "TestApp Exchange Web API",
        Description = "An ASP.NET Core Web API for finding best exchanges",
        Contact = new OpenApiContact
        {
            Name = "Developer",
            Url = new Uri("https://t.me/ivand_dev")
        },
    });
});

services.AddOptions();


services.Configure<BinanceOptions>(configuration.GetSection(BinanceOptions.Section));
services.Configure<KunaOptions>(configuration.GetSection(KunaOptions.Section));


services.Configure<List<string>>(configuration.GetSection("AllowedCoins"));

services
    .ConfigureValidation()
    .ConfigureAutoMapper()
    .ConfigureCryptoProviders();


services.AddSingleton<ExchangeRatesUpdater>();
services.AddSingleton<RatesUpdaterService>();
services.AddHostedService(p => p.GetRequiredService<RatesUpdaterService>());

//for endpoints
services.AddScoped<ExchangeService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

var handler = new ExchangeHandler();


app.MapGet("/estimate/", handler.Estimate)
    .Produces<EstimateDto>()
    .WithDescription("Returns a best estimate");

app.MapGet("/get-rates/", handler.GetRates)
    .Produces<List<RateDto>>()
    .WithDescription("Returns the rates");

app.Run();


