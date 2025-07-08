using CleanArch.Application;
using CleanArch.Infrastructure;
using CleanArch.Infrastructure.BackgroundJobs;
using CleanArch.Infrastructure.Data;
using CleanArch.Infrastructure.OpenTelemetry;
using CleanArch.Web.Api;
using CleanArch.Web.Api.Extensions;
using Hangfire;
using Serilog;

EnvironmentInspector.LoadAndPrintAll();

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// builder.Host.UseSerilog(
//     (context, loggerConfig) =>
//     {
//         loggerConfig.ReadFrom.Configuration(context.Configuration);
//     }
// );

// builder.Logging.AddOpenTelemetryLogging(builder.Configuration);

builder
    .Services.AddApplicationServices()
    .AddInfrastructureServices(builder.Environment, builder.Configuration)
    .AddWebServices();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "LocalCorsPolicy",
        policy =>
        {
            policy.WithOrigins("http://localhost:4000", "https://localhost:4000").AllowAnyHeader().AllowAnyMethod();
        }
    );
});

var app = builder.Build();

app.UseCors("LocalCorsPolicy");

app.MapDefaultEndpoints();

app.UseExceptionHandler();

await app.InitialiseDatabaseAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker"))
{
    app.UseHangfireDashboard("/hangfire", new DashboardOptions { Authorization = [new NoAuthorizationFilter()] });
    app.UseSwaggerWithUi();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapHealthChecks("/health");

app.UseHttpsRedirection();

app.UseRequestContextLogging();

//app.UseSerilogRequestLogging();

app.UseStaticFiles();

app.Map("/", () => Results.Redirect("/api"));
app.MapEndpoints();
app.MapControllers();

app.InitializeBackgroundJobs();

app.Run();
