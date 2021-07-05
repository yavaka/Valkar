namespace Infrastructure
{
    using Infrastructure.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ValkarDbContext : IdentityDbContext<User>
    {
        public ValkarDbContext(DbContextOptions<ValkarDbContext> options)
            : base(options)
        {
        }
    }
}
