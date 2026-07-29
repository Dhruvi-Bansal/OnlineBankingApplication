using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineBankingApplication.DAL;
using OnlineBankingApplication.Models;
using OnlineBankingApplication.Repositories;

namespace OnlineBankingApplication
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Connection String
            var cs = builder.Configuration.GetConnectionString("conn");

            // Database Context
            builder.Services.AddDbContext<OnlineBankingDbContext>(options =>
                options.UseSqlServer(cs));
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(cs));
            // Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;

                options.User.RequireUniqueEmail = true;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            })

            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
            builder.Services.AddScoped<IChequeBookRepo, ChequeBookRepo>();


            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
            builder.Services.AddScoped<IBankAccountRepo, BankAccountRepo>();
            builder.Services.AddScoped<IBeneficiaryRepo, BeneficiaryRepo>();    
            builder.Services.AddScoped<ITransactionRepo, TransactionRepo>();
            builder.Services.AddScoped<IProfileRepo, ProfileRepo>();
            builder.Services.AddScoped<IProfileUpdateRepo, ProfileUpdateRepo>();

            var app = builder.Build();

            // Seed Roles & Admin
            using (var scope = app.Services.CreateScope())
            {
                await DbInitializer.Seed(scope.ServiceProvider);
            }


            // Configure HTTP Pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseRouting();

            app.UseAuthentication();      
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