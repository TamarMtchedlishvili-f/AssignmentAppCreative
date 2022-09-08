using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using AssignmentAppCreativeUI;
using Serilog;
using Serilog.Core;
using Serilog.Extensions.Logging;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:1529/") });
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:1529/") });
// builder.Services.AddHttpClient<HttpClient>(sp =>
// {
//     sp.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
// });

var levelSwitch = new LoggingLevelSwitch();
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.ControlledBy(levelSwitch)
    .Enrich.WithProperty("InstanceId", Guid.NewGuid().ToString("n"))
    .WriteTo.Console()
    // .WriteTo.BrowserHttp(endpointUrl: $"{builder.HostEnvironment.BaseAddress}ingest", controlLevelSwitch: levelSwitch)
    .CreateLogger();

// /* this is used instead of .UseSerilog to add Serilog to providers */
builder.Logging.AddProvider(new SerilogLoggerProvider());

await builder.Build().RunAsync();
