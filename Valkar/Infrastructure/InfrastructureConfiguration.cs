namespace Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

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
#if DEBUG
            return services.AddDbContext<ValkarDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

#elif RELEASE
            return services.AddDbContext<ValkarDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("HostingConnection"));
            });         
#endif
        }
    }
}
