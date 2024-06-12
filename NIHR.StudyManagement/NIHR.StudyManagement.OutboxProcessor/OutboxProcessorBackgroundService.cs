using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NIHR.StudyManagement.Domain.Abstractions;
using NIHR.StudyManagement.OutboxProcessor.Configuration;

namespace NIHR.StudyManagement.OutboxProcessor
{
    /// <summary>
    /// Background service to initiate the IOuboxProcessor.ProcessAsync() operation which moves entries from
    /// the outbox to kafka.
    /// </summary>
    public class OutboxProcessorBackgroundService : BackgroundService
    {
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly OutboxProcessorSettings _settings;
        private readonly ILogger<OutboxProcessorBackgroundService> _logger;

        public OutboxProcessorBackgroundService(IHostApplicationLifetime hostApplicationLifetime,
            IServiceScopeFactory serviceScopeFactory,
            IOptions<OutboxProcessorSettings> options,
            ILogger<OutboxProcessorBackgroundService> logger)
        {
            this._hostApplicationLifetime = hostApplicationLifetime;
            this._scopeFactory = serviceScopeFactory;
            this._settings = options.Value;
            this._logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var outboxProcessor = scope.ServiceProvider.GetService<IOutboxProcessor>();

                        if (outboxProcessor == null)
                        {
                            // log failure of service initialisation.
                            _logger.LogError("Failed to initialise OutboxProcessor service.");

                            return;
                        }

                        _logger.LogInformation("OutboxProcessor running");

                        await outboxProcessor.ProcessAsync(cancellationToken);

                        _logger.LogInformation($"OutboxProcessor sleeping for {_settings.SleepInterval} seconds. Next run at {DateTime.Now.AddSeconds(_settings.SleepInterval).ToString("dd-MM-yyyy HH-mm-ss")}.");

                        await Task.Delay(_settings.SleepInterval* 1000);
                    }
                }
            }
            finally
            {
                // Terminate host application when exiting.
                _hostApplicationLifetime.StopApplication();
            }
        }
    }
}
