using Microsoft.EntityFrameworkCore;

namespace TaskManagementSystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add Entity Framework Core with your connection string
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Add ASP.NET Core Identity (if you need authentication)
            // services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

            // Add CORS policy (customize as needed)
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            services.AddControllers();

            // Add other services, repositories, or dependency injections as needed
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Add production error handling and logging here
            }

            // Enable CORS (customize policy name if needed)
            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseRouting();

            // Use authentication middleware here (if needed)
            // app.UseAuthentication();
            // app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
