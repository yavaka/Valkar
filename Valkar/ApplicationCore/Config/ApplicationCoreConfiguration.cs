﻿namespace ApplicationCore.Config
{
    using ApplicationCore;
    using ApplicationCore.Services.Admin;
    using ApplicationCore.Services.Company;
    using ApplicationCore.Services.Driver;
    using ApplicationCore.Services.Email;
    using ApplicationCore.Services.File;
    using ApplicationCore.Services.Google.ReCaptcha;
    using ApplicationCore.Services.GoogleDriveAPI;
    using ApplicationCore.Services.Identity;
    using ApplicationCore.Services.Mapper;
    using ApplicationCore.Services.PDFDocument;
    using ApplicationCore.Services.WorkingDay;
    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Drive.v3;
    using Google.Apis.Services;
    using Infrastructure;
    using Infrastructure.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public static class ApplicationCoreConfiguration
    {
        public static IServiceCollection AddApplicationCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApplicationCoreOptions>(configuration);
            
            return services.AddIdentityService()
                .AddApplicationCookie(configuration)
                .AddEmailSender(configuration)
                .AddGoogleDriveAPI()
                .AddTransient<IMapperService, MapperService>()
                .AddTransient<IDriverService, DriverService>()
                .AddTransient<IFileService, FileService>()
                .AddTransient<IAdminService, AdminService>()
                .AddTransient<IWorkingDayService, WorkingDayService>()
                .AddTransient<ICompanyService, CompanyService>()
                .AddTransient<IDocumentService, DocumentService>()
                .AddSingleton<IGoogleReCaptchaService, GoogleReCaptchaService>();
        }

        /// <summary>
        /// Identity config
        /// </summary>
        private static IServiceCollection AddIdentityService(this IServiceCollection services)
        {
            // password config
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

            // Token config
            services.Configure<DataProtectionTokenProviderOptions>(opt =>
               opt.TokenLifespan = TimeSpan.FromHours(24));

            // register identity service
            services.AddTransient<IIdentityService, IdentityService>();

            return services;
        }

        /// <summary>
        /// Cookies consent and config
        /// </summary>
        private static IServiceCollection AddApplicationCookie(this IServiceCollection services, IConfiguration configuration)
        {
            // cookies consent
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential 
                // cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
            });

            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = configuration["ApplicationSettings:LoginPath"];
                config.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                config.AccessDeniedPath = configuration["ApplicationSettings:AccessDenied"];
            });

            return services;
        }

        /// <summary>
        /// Email sender
        /// </summary>
        private static IServiceCollection AddEmailSender(this IServiceCollection services, IConfiguration configuration)
        {
            // Fetch EmailConfiguration section from appsettings.json
            var emailConfig = configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();

            services.AddSingleton(emailConfig);
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }

        /// <summary>
        /// Google Drive API
        /// </summary>
        private static IServiceCollection AddGoogleDriveAPI(this IServiceCollection services)
        {
            services.AddTransient(s =>
            {
                var credential = GoogleCredential.FromFile("Config\\valkar-service-account.json")
                    .CreateScoped(new[] { DriveService.Scope.Drive });

                var service = new DriveService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "valkar"
                });

                return service;
            });

            services.AddTransient<IGoogleDriveAPIService, GoogleDriveAPIService>();

            return services;
        }
    }
}
