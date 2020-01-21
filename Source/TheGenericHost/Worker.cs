namespace TheGenericHost
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using TheGenericHost.Options;
    using TheGenericHost.Services;

    public class Worker : BackgroundService
    {
        private readonly IOptions<AppOptions> appOptions;
        private readonly IClockService clockService;
        private readonly ILogger<Worker> logger;

        public Worker(
            IOptions<AppOptions> appOptions,
            IClockService clockService,
            ILogger<Worker> logger)
        {
            this.appOptions = appOptions;
            this.clockService = clockService;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                this.logger.LogInformation("Worker running at: {time}", this.clockService.Now);
                await Task.Delay(this.appOptions.Value.Delay, cancellationToken);
            }
        }
    }
}
