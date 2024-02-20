using Blazored.LocalStorage;
using BlazorPoultryDashboard.Application.Database.SQLCe;
using BlazorPoultryDashboard.Application.Middleware;
using BlazorPoultryDashboard.Application.Notifications;
using BlazorPoultryDashboard.Application.Services;
using BlazorPoultryDashboard.Components;
using BlazorPoultryDashboard.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

// Register HttpClient service
builder.Services.AddHttpClient();

// Add configuration
builder.Configuration.AddJsonFile("appsettings.json");
// Get connection string from configuration
string? connectionString = builder.Configuration.GetConnectionString("BirdDbContextConnection");

// Configure the services
builder.Services.AddDbContext<BirdDbContext>(options =>
    options.UseSqlite(connectionString), ServiceLifetime.Scoped);

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddSingleton<IConfigurationRoot>(builder.Configuration as IConfigurationRoot);

builder.Services.AddScoped<BirdDataWriter>();
builder.Services.AddTransient<BirdDataReceiver>();
builder.Services.AddTransient<ReportDataReceiver>();
builder.Services.AddSingleton<IDbEventNotifier, DbEventNotifier>();
builder.Services.AddSingleton<IReportEventNotifier, ReportEventNotifier>();
builder.Services.AddSingleton<IThresholdLimitNotifier, ThresholdLimitNotifier>();
builder.Services.AddHostedService<BindWebApiWorkerService>(); // Web API functions' results will be stored in the database in every 1 second.
builder.Services.AddHostedService<ReportingWorkerService>(); //  Reportingn results, including the average rate of bird weight change of {averageRate} and various key performance indicators for bird weight such as minimum weight of {minWeight},
                                                             //  maximum weight of {maxWeight}, and median weight of {medianWeight}, will be stored into the database in every 5 minutes.

builder.Services.AddBlazoredLocalStorage(); // Add this line to register the LocalStorageService

var app = builder.Build();

SQLitePCL.Batteries.Init();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

//Middleware Configurations.
app.UseErrorHandlingMiddleware();


await app.RunAsync();
