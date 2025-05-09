
using Microsoft.EntityFrameworkCore;
using ReportApi.Application.Interfaces;
using ReportApi.Application.Services;
using ReportApi.Infrastructure.Context;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ReportDbContext>(options =>
   options.UseNpgsql(builder.Configuration.GetConnectionString("WebApiDatabase")));

builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<BackgroundService, ReportConsumerService>();
builder.Services.AddHttpClient(); 
 


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
