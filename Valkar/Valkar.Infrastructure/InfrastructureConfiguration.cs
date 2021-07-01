namespace Valkar.Infrastructure
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using Valkar.Application.Interfaces.Identity;
    using Valkar.Infrastructure.Identity;
    using Valkar.Infrastructure.Persistence;

    public static class InfrastructureConfiguration
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services)
            => services.AddAspNetIdentity();

        private static IServiceCollection AddAspNetIdentity(
            this IServiceCollection services)
        {
            services
                .AddIdentity<User, IdentityRole>(opt =>
                {
                    opt.Password.RequiredLength = 6;
                    opt.Password.RequireDigit = true;
                    opt.Password.RequireLowercase = true;
                    opt.Password.RequireUppercase = true;
                    opt.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<ValkarDbContext>();

            services
                .AddTransient<IIdentity, IdentityService>();

            return services;
        }
    }
}
