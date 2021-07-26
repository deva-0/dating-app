using System;
using System.IO;
using System.Reflection;
using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

            services.AddScoped<ITokenService, TokenService>(); // JWT 
            services.AddScoped<IPhotoService, PhotoService>(); // Cloudinary photo service
            services.AddScoped<LogUserActivity>(); // Updates user last active date 
            services.AddScoped<IUserRepository, UserRepository>(); // For handling user accounts 
            services.AddScoped<ILikesRepository, LikesRepository>(); // For 'likes' functionality
            services.AddScoped<IMessageRepository, MessageRepository>(); // for user messages 
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly); // For AutoMapper
            // For SQLite db connection (_config=appsettings.Development.json)
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1", new OpenApiInfo
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


                // Authenticate with JWT Token
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
                setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                setup.IncludeXmlComments(xmlPath);
            });

            return services;
        }
    }
}