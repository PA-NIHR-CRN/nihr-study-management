using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NIHR.StudyManagement.Domain.Abstractions;
using NIHR.StudyManagement.Domain.Configuration;
using NIHR.StudyManagement.Domain.Models;

namespace NIHR.StudyManagement.Infrastructure.MessageBus
{

    public class StudyManagementKafkaMessageProducer : IStudyEventMessagePublisher
    {
        private readonly ILogger<StudyManagementKafkaMessageProducer> _logger;
        private readonly MessageBusSettings _settings;

        public StudyManagementKafkaMessageProducer(ILogger<StudyManagementKafkaMessageProducer> logger,
            IOptions<MessageBusSettings> settings)
        {
            _logger = logger;
            _settings = settings.Value;

            _logger.LogInformation($"MSK settings BootstrapServers: {_settings.BootstrapServers}. Topic: {_settings.Topic}");
        }

        public async Task PublishAsync(string eventType,
            string sourceSystemName,
            GovernmentResearchIdentifier governmentResearchIdentifier,
            CancellationToken cancellationToken)
        {
            var nsipMessage = new NsipMessage<GovernmentResearchIdentifier>(governmentResearchIdentifier)
            {
                NsipEventId = "1",
                NsipEventSourceSystemId = sourceSystemName,
                NsipEventType = eventType,
            };

            await Publish(nsipMessage, cancellationToken);
        }

        private async Task Publish<TEventType>(NsipMessage<TEventType> nsipMessage,
            CancellationToken cancellationToken)
            where TEventType : new()
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = _settings.BootstrapServers
            };

            var nsipMessageJson = System.Text.Json.JsonSerializer.Serialize(nsipMessage);

            using (var producer = new ProducerBuilder<Null, string>(producerConfig).Build())
            {
                var msg = new Message<Null, string>() { Value = nsipMessageJson };

                _logger.LogInformation($"Producer: About to publish {nsipMessageJson}");

                var deliveryReport = await producer.ProduceAsync(_settings.Topic, msg, cancellationToken: cancellationToken);

                _logger.LogInformation("Delivered message to Topic={Topic} " +
                    "Offset={Offset} " +
                    "Key={Key} " +
                    "Partition={Partition} " +
                    "Status={Status} " +
                    "Value={Value}",
                    _settings.Topic,
                    deliveryReport.Offset.Value,
                    "PoC" ?? "NULL",
                    deliveryReport.TopicPartition.Partition.Value,
                    deliveryReport.Status,
                    nsipMessageJson);
            }
        }
    }
}
