using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SantanderTestWebAPI.Interfaces;
using SantanderTestWebAPI.Models;
using SantanderTestWebAPI.Services;
using System;

namespace SantanderTestWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configure settings from appsettings.json
            builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("HackerNewsApi"));

            // Add HttpClient and HackerNewsService
            builder.Services.AddHttpClient<IHackerNewsFetcher, HackerNewsFetcher>();
            builder.Services.AddScoped<IMemoryCacheService, MemoryCacheService>();
            builder.Services.AddMemoryCache();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Use global exception handling middleware
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = 500; // Internal Server Error

                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = exceptionHandlerPathFeature.Error;

                    // Log the exception or take any other necessary actions
                    // For example, log the exception using a logging framework like Serilog:
                    // Log.Error(exception, "An unexpected error occurred.");

                    await context.Response.WriteAsync($"An unexpected error occurred: {exception.Message}");
                });
            });


            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
