namespace ApplicationCore
{
    using ApplicationCore.Services.Admin;
    using ApplicationCore.Services.Driver;
    using ApplicationCore.Services.Email;
    using ApplicationCore.Services.File;
    using ApplicationCore.Services.Identity;
    using ApplicationCore.Services.Mapper;
    using ApplicationCore.Services.PDFGenerator;
    using ApplicationCore.Services.WorkingDay;
    using DinkToPdf;
    using DinkToPdf.Contracts;
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

            services.AddTransient<IFileService, FileService>();

            services.AddEmailSender(configuration);

            services.AddTransient<IAdminService, AdminService>();
            
            services.AddTransient<IWorkingDayService, WorkingDayService>();

            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.AddTransient<IPDFGeneratorService, PDFGeneratorService>();
            
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
                .AddEntityFrameworkStores<ValkarDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(opt =>
               opt.TokenLifespan = TimeSpan.FromHours(24));

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
                config.AccessDeniedPath = configuration["ApplicationSettings:AccessDenied"];
            });

        private static IServiceCollection AddEmailSender(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Fetch EmailConfiguration section from appsettings.json
            var emailConfig = configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();

            services.AddSingleton(emailConfig);
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
