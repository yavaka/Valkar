namespace ApplicationCore
{
    using ApplicationCore.Services.Driver;
    using ApplicationCore.Services.Identity;
    using ApplicationCore.Services.Mapper;
    using Infrastructure;
    using Infrastructure.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public static class ApplicationCoreConfiguration
    {
        public static IServiceCollection AddApplicationCore(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddIdentityService();
            services.AddApplicationCookie(configuration);

            services.AddTransient<IMapperService, MapperService>();

            services.AddTransient<IDriverService, DriverService>();

            return services;
        }

        private static IServiceCollection AddIdentityService(
            this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(opt =>
                {
                    opt.Password.RequiredLength = 6;
                    opt.Password.RequireDigit = true;
                    opt.Password.RequireLowercase = true;
                    opt.Password.RequireUppercase = true;   
                    opt.Password.RequireNonAlphanumeric = false;
                    opt.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ValkarDbContext>();

            services.AddTransient<IIdentityService, IdentityService>();

            return services;
        }

        private static IServiceCollection AddApplicationCookie(
            this IServiceCollection services,
            IConfiguration configuration)
            => services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = configuration["ApplicationSettings:LoginPath"];
                config.ExpireTimeSpan = TimeSpan.FromMinutes(10);
            });
    }
}
