using CardanoAssignment.Convertors;
using CardanoAssignment.Enrichments;
using CardanoAssignment.Extensions;
using CardanoAssignment.Models;
using CardanoAssignment.Processors;
using CardanoAssignment.Repositories;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var isDevelopment = environmentName == "Development";
ConfigurationManager configuration = builder.Configuration;
    configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: isDevelopment)
    .AddJsonFile($"appsettings.{environmentName}.json", true, isDevelopment)
    .AddEnvironmentVariables()
    .AddEnvironmentVariables(prefix: "ASPNETCORE_")
    .Build();
var gleiApiConfiguration = configuration.GetSection("GleiApiConfiguration").Get<GleiApiConfiguration>();
builder.Services.AddControllers();
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();
builder.Host.UseSerilog();
builder.Services.AddPolicyRegistries();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGleiHttpClient(gleiApiConfiguration);
builder.Services.AddSingleton<ICsvReaderFactory, CsvReaderFactory>();
builder.Services.AddSingleton<ICsvWriterFactory, CsvWriterFactory>();
builder.Services.AddScoped<ICsvConvertor, CsvConvertor>();
builder.Services.AddScoped<IGleifRepository, GleifRepository>();
builder.Services.AddScoped<ILeiDataEnrichmentHandler, LeiDataEnrichmentHandler>();
builder.Services.AddScoped<IDataSetEnrichmentProcessor, DataSetEnrichmentProcessor>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();