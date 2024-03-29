﻿namespace Web.Config
{
    using ApplicationCore.Config;
    using Infrastructure;
    using Microsoft.Extensions.DependencyInjection;

    public static class WebConfiguration
    {
        public static IServiceCollection AddWeb(this IServiceCollection services)
        {
            services.AddAutoMapper(
                typeof(Startup),
                typeof(ApplicationCoreConfiguration),
                typeof(InfrastructureConfiguration));

            return services;
        }
    }
}
