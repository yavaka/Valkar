namespace Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public static class InfrastructureConfiguration
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
            => services.AddDbContext(configuration);

        private static IServiceCollection AddDbContext(
           this IServiceCollection services,
           IConfiguration configuration)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                return services.AddDbContext<ValkarDbContext>(opt =>
                {
                    opt.UseSqlServer(configuration.GetConnectionString("AzureConnection"));
                });
            }
            else
            {
                return services.AddDbContext<ValkarDbContext>(opt =>
                {
                    opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                });
            }
        }
    }
}
