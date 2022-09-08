using AssignmentAppCreative.WebAPI.Interfaces;
using AssignmentAppCreative.WebAPI.Services;
using StackExchange.Redis;

namespace AssignmentAppCreative.WebAPI;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpClient();
        // .AddHttpClient(client => client.BaseAddress = new Uri("https://localhost:1549"));

        services.AddControllers();
        services.AddSingleton<IWeatherService, WeatherService>();
        services.AddSingleton<ICacheManager, CacheManager>();
        services.AddSingleton<IAwsSecretManager, AwsSecretManager>();
        services.AddSingleton<IWeatherDataRetriever, WeatherDataRetriever>();
        services.AddSingleton(_ =>
        {
            var configSection = Configuration.GetSection("RedisEndpoint");

            return ConnectionMultiplexer.Connect(
                new ConfigurationOptions
                {
                    EndPoints = { configSection.GetSection("Address").Value },
                    Password = configSection.GetSection("Password").Value
                });
        });

        services.AddControllersWithViews();
        services.AddRazorPages();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();
        app.UseCors("CorsPolicy");

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            endpoints.MapControllers();
            endpoints.MapFallbackToFile("index.html");
        });

        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();
    }
}