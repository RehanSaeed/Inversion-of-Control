namespace TheGenericHost
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using TheGenericHost.Options;
    using TheGenericHost.Services;

    public class Program
    {
        public static Task Main(string[] args) => CreateHostBuilder(args).Build().RunAsync();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(ConfigureServices);

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services) =>
            services
                .AddHostedService<Worker>()
                .Configure<AppOptions>(context.Configuration)
                .AddSingleton<IClockService, ClockService>()
                .AddSingleton<ISingletonService, SingletonService>()
                .AddScoped<IScopedService, ScopedService>()
                .AddTransient<ITransientService, TransientService>();
    }
}
