using Microsoft.Net.Http.Headers;
using TM_LambdaASP.NETCoreWebAPI.Controllers;
using TM_LambdaASP.NETCoreWebAPI.Controllers.WeatherServiceRelated;

namespace AssignmentAppCreative
{
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
            services
                .AddHttpClient<HttpClient>(client => client.BaseAddress = new Uri("https://localhost:1529"));
            // services.AddCors(options =>
            // {
            //     var origins = new[] { "https://localhost:7005", "https://localhost:1529" }.ToArray();
            //     
            //     options.AddDefaultPolicy(builder => 
            //         builder.WithOrigins(origins)
            //             .AllowAnyMethod()
            //             .AllowAnyHeader());
            //     
            //     options.AddPolicy("AllowHeaders", builder =>
            //     {
            //         builder.WithOrigins(origins)
            //             .WithHeaders(HeaderNames.ContentType, HeaderNames.Server, HeaderNames.AccessControlAllowHeaders, 
            //                 HeaderNames.AccessControlExposeHeaders, "x-custom-header", "x-path", "x-record-in-use", HeaderNames.ContentDisposition);
            //     });
            // }); 
            
            services.AddControllers();
            services.AddSingleton<IWeatherService, WeatherService>();
            services.AddSingleton<ICacheManager, CacheManager>();
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
}