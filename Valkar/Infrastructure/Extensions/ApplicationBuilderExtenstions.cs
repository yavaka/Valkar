namespace Infrastructure.Extensions
{
    using Infrastructure.Common.Global;
    using Infrastructure.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System.Threading.Tasks;
    using System.Linq;

    public static class ApplicationBuilderExtenstions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using var services = app.ApplicationServices.CreateScope();

            var dbContext = services.ServiceProvider.GetService<ValkarDbContext>();

            if (dbContext.Database.GetPendingMigrations().Any())
            {
                dbContext.Database.Migrate();
            }
        }

        public static async Task AddRolesAndAdminAsync(this IApplicationBuilder app)
        {
            using var services = app.ApplicationServices.CreateScope();

            var roleManager = services.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (await roleManager.RoleExistsAsync(Role.Admin) is false)
            {
                // Create Admin role
                var result = await roleManager.CreateAsync(
                    new IdentityRole(Role.Admin));

                if (result.Succeeded)
                {
                    var userManager = services.ServiceProvider.GetRequiredService<UserManager<User>>();
                    // Get AdminCredentials section in appsettings.json
                    var adminCredentials = services.ServiceProvider.GetRequiredService<IConfiguration>();
                    // Get admin by email 
                    var admin = await userManager.FindByEmailAsync(adminCredentials["AdminCredentials:Email"]);

                    // Create admin
                    if (admin is null)
                    {
                        admin = new User
                        {
                            Email = adminCredentials["AdminCredentials:Email"],
                            UserName = adminCredentials["AdminCredentials:UserName"],
                            EmailConfirmed = true
                        };

                        result = await userManager.CreateAsync(admin, adminCredentials["AdminCredentials:Password"]);

                        // Assign admin to role
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(admin, Role.Admin);
                        }
                    }
                }
            }
            // Add driver role
            if (await roleManager.RoleExistsAsync(Role.Driver) is false)
            {
                await roleManager.CreateAsync(
                    new IdentityRole(Role.Driver));
            }
        }
    }
}
