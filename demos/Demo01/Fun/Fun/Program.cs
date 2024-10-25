using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;

#region Default template
//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllersWithViews();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.Run();

#endregion

#region Step 1

//// Step 1 - Create a Serilog Logger 
//Log.Logger = new LoggerConfiguration()
//    .WriteTo.Console()
//    .CreateLogger();

//// Step 1 - Add try/catch
//try
//{
//    Log.Information("Starting web application");

//    var builder = WebApplication.CreateBuilder(args);

//    //Step 1 - Add this line
//    builder.Services.AddSerilog();

//    // Add services to the container.
//    builder.Services.AddControllersWithViews();

//    var app = builder.Build();

//    // Configure the HTTP request pipeline.
//    if (!app.Environment.IsDevelopment())
//    {
//        app.UseExceptionHandler("/Home/Error");
//        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//        app.UseHsts();
//    }

//    app.UseHttpsRedirection();
//    app.UseStaticFiles();
//    app.UseRouting();
//    app.UseAuthorization();

//    app.MapControllerRoute(
//        name: "default",
//        pattern: "{controller=Home}/{action=Index}/{id?}");

//    app.Run();
//}
//catch (Exception ex)
//{
//    Log.Fatal(ex, "Application terminated unexpectedly");
//}
//finally
//{
//    Log.CloseAndFlush();
//}

#endregion

#region Step 2

//// Step 2
//Log.Logger = new LoggerConfiguration()
//    .WriteTo.Console()
//    .CreateBootstrapLogger(); // <!-- changed from CreateLogger to CreateBootstrapLogger

//try
//{
//    Log.Information("Starting web application");

//    var builder = WebApplication.CreateBuilder(args);

//    // Step 2, replace old line
//    // builder.Services.AddSerilog(); // <-- Add this line
//    // with this
//    builder.Services.AddSerilog(((services, lc) => lc

//        // read config from appsettings.json
//        .ReadFrom.Configuration(builder.Configuration)

//        // Optionally you can also do this in code via....
//        //.MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
//        //.MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
//        //.MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)

//        .ReadFrom.Services(services)
//        .Enrich.FromLogContext()
//        .WriteTo.Console())
//    );

//    // Add services to the container.
//    builder.Services.AddControllersWithViews();

//    var app = builder.Build();

//    // Configure the HTTP request pipeline.
//    if (!app.Environment.IsDevelopment())
//    {
//        app.UseExceptionHandler("/Home/Error");
//        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//        app.UseHsts();
//    }

//    app.UseHttpsRedirection();
//    app.UseStaticFiles();

//    //Step 2 - be sure to put this after static file handler, you get a LOT less noise
//    app.UseSerilogRequestLogging(); // <-- Add this line

//    app.UseRouting();

//    app.UseAuthorization();

//    app.MapControllerRoute(
//        name: "default",
//        pattern: "{controller=Home}/{action=Index}/{id?}");

//    app.Run();
//}
//catch (Exception ex)
//{
//    Log.Fatal(ex, "Application terminated unexpectedly");
//}
//finally
//{
//    Log.CloseAndFlush();
//}
#endregion

#region Step 3
//add Serilog.Formatting.Compact nuget package

//Step 3
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(new RenderedCompactJsonFormatter()) // <!--add JsonFormatter
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    // Step 3
    builder.Services.AddSerilog(((services, lc) => lc
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
      //  .WriteTo.Console())
    //.WriteTo.Console(new RenderedCompactJsonFormatter()))  // <!--add JsonFormatter
    .WriteTo.Console(theme: AnsiConsoleTheme.Code))
    );

    // Add services to the container.
    builder.Services.AddControllersWithViews();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseSerilogRequestLogging();
    app.UseRouting();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
#endregion

// Go to HomeController now