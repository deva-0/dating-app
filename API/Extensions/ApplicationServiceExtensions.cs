using System;
using System.IO;
using System.Reflection;
using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace API.Extensions
{
    /// <summary>
    ///     Separates arbitrary services from required ones inside Startup class. Has only aesthetic value.
    /// </summary>
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration config)
        {
            // Pass API token and secret to cloudinary settings
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            // JWT 
            services.AddScoped<ITokenService, TokenService>();
            // Cloudinary photo service
            services.AddScoped<IPhotoService, PhotoService>();
            // Abstraction over DbContext 
            services.AddScoped<IUserRepository, UserRepository>();
            // For AutoMapper
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            // For SQLite db connection (_config=appsettings.Development.json)
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Dating App API",
                    Description = "Example of dating app API",
                    TermsOfService = new System.Uri("https://github.com/deva297/dating-app/blob/main/LICENSE"),
                    Contact = new OpenApiContact
                    {
                        Name = "Adrian Zaręba",
                        Email = "dev.adrianzareba@gmail.com",
                        Url = new System.Uri("https://github.com/deva297/dating-app")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT",
                        Url = new System.Uri("https://github.com/deva297/dating-app/blob/main/LICENSE"),
                    }
                });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            return services;
        }
    }
}