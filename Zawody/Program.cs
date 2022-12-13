using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Zawody.Data;
using Zawody.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Zawody.Authorization;
using Zawody.Services;
using Zawody.Middleware;
using System.Web.Mvc;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNLog();
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
/*builder.Services.AddDefaultIdentity<ZawodyUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();builder.Services.AddDatabaseDeveloperPageExceptionFilter();*/

/*builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();*/
builder.Services.AddIdentity<ZawodyUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultUI()
            .AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication()
   .AddGoogle(options =>
   {
       IConfigurationSection googleAuthNSection =
       builder.Configuration.GetSection("Authentication:Google");
       options.ClientId = googleAuthNSection["ClientId"];
       options.ClientSecret = googleAuthNSection["ClientSecret"];
   });
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});
builder.Services.AddScoped<AutoMigration>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();



using (IServiceScope? scope = app.Services.CreateScope())
{
    IServiceProvider? services = scope.ServiceProvider;
    ILoggerFactory? loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        ApplicationDbContext? context = services.GetRequiredService<ApplicationDbContext>();
        UserManager<ZawodyUser>? userManager = services.GetRequiredService<UserManager<ZawodyUser>>();
        RoleManager<IdentityRole>? roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await ContextSeed.SeedRolesAsync(userManager, roleManager);
        await ContextSeed.SeedSuperAdminAsync(userManager, roleManager);
        /*context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;*/
        ContextSeed.Initialize(context);
    }
    catch (Exception ex)
    {
        ILogger<Program>? logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

/*app.MapControllerRoute(name: "team",
                pattern: "team/{*player}",
                defaults: new { controller = "Teams", action = "Index" });*/
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

#pragma warning disable CA1050 // Declare types in namespaces
public partial class Program
{
}
#pragma warning restore CA1050 // Declare types in namespaces