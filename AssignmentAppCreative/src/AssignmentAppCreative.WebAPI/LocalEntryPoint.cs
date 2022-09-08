namespace AssignmentAppCreative
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
                // .ConfigureAppConfiguration((hostingContext, config) =>
                // {
                //     var environmentName = hostingContext.HostingEnvironment.EnvironmentName;
                //     var appName = hostingContext.HostingEnvironment.ApplicationName;
                //     config.AddSecretsManager(region: RegionEndpoint.USEast1,
                //         configurator: options =>
                //         {
                //             var envAppName = $"{environmentName}_{appName}_";
                //             options.SecretFilter = entry => entry.Name.StartsWith(envAppName);
                //             options.KeyGenerator = (_, s) => s
                //                 .Replace(envAppName, String.Empty)
                //                 .Replace("__", ":");
                //         });
                // })
                //
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}