//using Microsoft.Azure.Functions.Worker.Builder;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using sample.Utilities;

//var builder = FunctionsApplication.CreateBuilder(args);
////var host = new HostBuilder()
////    .ConfigureFunctionsWorkerDefaults()
////    .Build();
////builder.ConfigureFunctionsWebApplication();


////// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
////// builder.Services
//////     .AddApplicationInsightsTelemetryWorkerService()
//////     .ConfigureFunctionsApplicationInsights();

////builder.Build().Run();

//builder.Services.AddSingleton<CsvProcessor>(provider =>
//{
//    // Initialize CsvProcessor with the mapping path
//    return new CsvProcessor("C:\\Users\\Inno\\OneDrive - Acumant Technologies Private Limited\\Desktop\\sample2\\mappings.json");
//});

//var host = builder.Build();
//host.Run();



using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using sample.Utilities;

var builder = FunctionsApplication.CreateBuilder(args);

builder.Services.AddSingleton(provider =>
{
    return new CsvProcessor("C:\\Users\\Inno\\OneDrive - Acumant Technologies Private Limited\\Desktop\\sample2\\mappings.json");
});

builder.Services.AddHttpClient();

var host = builder.Build();
host.Run();

