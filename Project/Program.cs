using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Project.Data;
using Project.Permissions;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        AddServices(builder);

        var app = builder.Build();
        AddSeedData(app);
        AddMiddlewares(app);
        app.Run();
    }
    private static void AddServices(WebApplicationBuilder builder)
    {
        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddDbContext<SauceContext>(o =>
            o.UseSqlServer(
                builder.Configuration.GetConnectionString("SauceContext") ??
                throw new InvalidOperationException("Connection string 'SauceContext' not found."))
        );
        
        builder.Services.AddIdentityCore<IdentityUser<int>>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });
        builder.Services.ConfigureApplicationCookie(options =>
        {
            // Cookie settings
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            options.LoginPath = "/Identity/Account/Login";
            options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            options.SlidingExpiration = true;
        });
    }
    private static void AddSeedData(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            using (var db = new SauceContext(scope.ServiceProvider.GetRequiredService<DbContextOptions<SauceContext>>()))
            {
                var seeder = new DataSeeder(db);
                seeder.SeedAllModels();
            }
        }
    }
    private static void AddMiddlewares(WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for
            // production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseMiddleware<PermissionsMiddleWare>();
        app.MapRazorPages();
    }
}