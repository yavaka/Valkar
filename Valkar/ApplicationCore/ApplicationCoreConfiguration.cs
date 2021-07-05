namespace ApplicationCore
{
    using ApplicationCore.Services.Identity;
    using Infrastructure;
    using Infrastructure.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public static class ApplicationCoreConfiguration
    {
        public static IServiceCollection AddApplicationCore(
            this IServiceCollection services)
        {
            services.AddIdentityService();
            services.AddAuthAndCookies();

            return services;
        }

        private static IServiceCollection AddAuthAndCookies(
            this IServiceCollection services)
            => services
                .AddAuthentication()
                .AddCookie(opt => 
                {
                    opt.LoginPath = "/Identity/Login";
                    opt.AccessDeniedPath = "/Identity/Login";
                    opt.ExpireTimeSpan = new TimeSpan(0, 10, 0);
                }).Services;

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
                })
                .AddEntityFrameworkStores<ValkarDbContext>();

            services.AddTransient<IIdentityService, IdentityService>();

            return services;
        }
    }
}
