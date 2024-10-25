using Lamar;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SynergisticLogging.Web.Data;
using SynergisticLogging.Web.External;
using SynergisticLogging.Web.External.RapidApi;
using SynergisticLogging.Web.Framework;
using SynergisticLogging.Web.Logging;

namespace SynergisticLogging.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureLogging(builder);

        builder.Host.UseLamar((context, registry) =>
        {
            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                                   throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            ConfigureIoC(registry, connectionString);
        });


        var app = builder.Build();

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

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.Run();

    }

    public static void ConfigureIoC(ServiceRegistry registry,string connectionString)
    {
        registry.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
        registry.AddSingleton<IDateTimeService, DateTimeService>();
        registry.AddDatabaseDeveloperPageExceptionFilter();
        registry.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();

        registry.For<IRapidApiClient>().Use<RapidApiClient>();
        registry.For<IDadJokesApiService>().Use<DadJokesApiService>();
        registry.AddControllersWithViews();
    }
    private static void ConfigureLogging(WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .CreateLogger();

        builder.Host.UseSerilog(Log.Logger);

        builder.Services.AddSingleton<MyLogger>();
    }
    
}