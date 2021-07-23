using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    /// <summary>
    /// Separates arbitrary services from required ones inside Startup class. Has only aesthetic value. 
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

            return services;
        }
    }
}