using Microsoft.EntityFrameworkCore;
using OnlineBankingApplication.Models;

namespace OnlineBankingApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            var cs = builder.Configuration.GetConnectionString("conn");
            builder.Services.AddDbContext<OnlineBankingDbContext>(Options => Options.UseSqlServer(cs));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Bank}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
