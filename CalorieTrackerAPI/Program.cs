using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using CalorieTrackerAPI.Services; // Make sure this matches the namespace of your services

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        // Register Application Insights
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        // Register your services
        services.AddSingleton<DataService>();           // Register DataService
        services.AddSingleton<CalorieTrackerService>(); // Register CalorieTrackerService
    })
    .Build();

host.Run();