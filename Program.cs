using CardanoAssignment.Extensions;
using CardanoAssignment.Models;
using CardanoAssignment.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

ConfigurationManager configuration = builder.Configuration;
var gleiApiConfiguration = configuration.GetSection("GleiApiConfiguration").Get<GleiApiConfiguration>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGleiHttpClient(gleiApiConfiguration);
builder.Services.AddScoped<IGleifRepository, GleifRepository>();
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