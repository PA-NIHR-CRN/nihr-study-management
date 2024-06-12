using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NIHR.StudyManagement.Domain.Abstractions;
using NIHR.StudyManagement.Domain.Configuration;
using NIHR.StudyManagement.Domain.Services;
using NIHR.StudyManagement.Infrastructure;
using NIHR.StudyManagement.Infrastructure.MessageBus;
using NIHR.StudyManagement.Infrastructure.Repository;
using NIHR.StudyManagement.OutboxProcessor.Configuration;

namespace NIHR.StudyManagement.OutboxProcessor
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

            await Host.CreateDefaultBuilder()
                .UseEnvironment(environmentName ?? "development")
                .ConfigureAppConfiguration((_, builder) =>
                {
                    //builder.AddJsonFile("appsettings.json")
                    //    .AddJsonFile($"appsettings.{environmentName}.json");
                    builder.Build();
                })
                .ConfigureServices((_, services) =>
                {
                    services.AddHostedService<OutboxProcessorBackgroundService>();

                    services.AddOptions<MessageBusSettings>().Bind(_.Configuration.GetSection("MessageBus"));
                    services.AddOptions<OutboxProcessorSettings>().Bind(_.Configuration.GetSection("OutboxProcessor"));

                    services.AddScoped<IStudyRegistryRepository, StudyRegistryRepository>();
                    services.AddScoped<IStudyRecordOutboxRepository, StudyRecordOutboxRepository>();
                    services.AddScoped<INsipGrisMessageHelper, StudyManagementKafkaMessageProducer>();
                    services.AddScoped<IOutboxProcessor, StudyRecordOutboxProcessor>();
                    services.AddScoped<IStudyEventMessagePublisher, StudyManagementKafkaMessageProducer>();

                    var outboxProcessorConfigurationSection = _.Configuration.GetSection("Data");
                    var databaseSettings = outboxProcessorConfigurationSection.Get<DatabaseSettings>();

                    services.AddDbContext<StudyRegistryContext>(options =>
                    {
                        // For local development, username/password included in connection string.
                        // For deployed lambda in AWS, password is retrieved from secret manager
                        var connectionString = "";

                        if (!string.IsNullOrEmpty(databaseSettings?.PasswordSecretName))
                        {
                            // Retrieve password from AWS Secrets.
                            var password = SharedApplicationnStartup.GetAwsSecretPassword(databaseSettings.PasswordSecretName);

                            connectionString = $"{databaseSettings.ConnectionString};password={password}";
                        }
                        else
                        {
                            connectionString = databaseSettings?.ConnectionString;
                        }

                        var serverVersion = ServerVersion.AutoDetect(connectionString);


                        options.UseMySql(connectionString, serverVersion);
                    });
                })
                .RunConsoleAsync();
        }
    }
}
