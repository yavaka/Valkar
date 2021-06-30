namespace Valkar.Infrastructure.Persistence
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Valkar.Domain.Models;
    using System.Reflection;

    public class ValkarDbContext : IdentityDbContext<User>
    {
        public ValkarDbContext(DbContextOptions<ValkarDbContext> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
