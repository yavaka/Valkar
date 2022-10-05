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
    using System.Collections.Generic;

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
                var result = await roleManager.CreateAsync(new IdentityRole(Role.Admin));

                if (result.Succeeded)
                {
                    var userManager = services.ServiceProvider.GetRequiredService<UserManager<User>>();

                    // Get AdminCredentials section in appsettings.json
                    var configuration = services.ServiceProvider.GetRequiredService<IConfiguration>();

                    var admins = configuration.GetSection("AdminCredentials").Get<List<AdminCredentials>>();

                    foreach (var a in admins)
                    {
                        // Get admin by email 
                        var admin = await userManager.FindByEmailAsync(a.Email);

                        // Create admin
                        if (admin is null)
                        {
                            admin = new User
                            {
                                Email = a.Email,
                                UserName = a.UserName,
                                EmailConfirmed = true
                            };

                            result = await userManager.CreateAsync(admin, a.Password);

                            // Assign admin to role
                            if (result.Succeeded)
                            {
                                await userManager.AddToRoleAsync(admin, Role.Admin);
                            }
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

        private class AdminCredentials
        {
            public string Email { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
        }
    }
}
