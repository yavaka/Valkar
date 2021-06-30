namespace Valkar.API
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Valkar.Domain.Models;
    using Valkar.Infrastructure.Persistence;

    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
            => _configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Valkar.API", Version = "v1" });
            });

            // DbContext
            services.AddDbContext<ValkarDbContext>(opt => 
            {
                opt.UseSqlServer(this._configuration.GetConnectionString("DefaultConnection"));
            });

            // Identity
            services
                .AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ValkarDbContext>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Valkar.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
