using Microsoft.Extensions.Logging;
using NIHR.StudyManagement.Domain.Abstractions;
using NIHR.StudyManagement.Domain.Constants;

namespace NIHR.StudyManagement.Domain.Services
{
    public class StudyRecordOutboxProcessor : IOutboxProcessor
    {
        private readonly IStudyRecordOutboxRepository _studyRecordOutboxRepository;
        private readonly IStudyEventMessagePublisher _studyEventMessagePublisher;
        private readonly ILogger<StudyRecordOutboxProcessor> _logger;

        public StudyRecordOutboxProcessor(
            IStudyRecordOutboxRepository studyRecordOutboxRepository,
            IStudyEventMessagePublisher studyEventMessagePublisher,
            ILogger<StudyRecordOutboxProcessor> logger)
        {
            this._studyRecordOutboxRepository = studyRecordOutboxRepository;
            this._studyEventMessagePublisher = studyEventMessagePublisher;
            this._logger = logger;
        }

        public async Task ProcessAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("OutboxProcessor starting ProcessAsync");

            // Read Outbox
            var numberOfOutboxItemsProcessed = 0;

            while (!cancellationToken.IsCancellationRequested)
            {
                var nextOutboxItemToProcess  = _studyRecordOutboxRepository.GetNextUnprocessedOutboxItem();

                if(nextOutboxItemToProcess == null)
                {
                    // Log
                    _logger.LogInformation("No entries found for processing in outbox table.");
                    return;
                }

                _logger.LogInformation($"Processing outbox record with id {nextOutboxItemToProcess.Id}");

                numberOfOutboxItemsProcessed++;

                _logger.LogDebug($"Processing outbox record with id {nextOutboxItemToProcess.Id} - mark as processing");

                // Mark item as "Processing"
                await _studyRecordOutboxRepository.UpdateOutboxItemStatusAsync(
                    nextOutboxItemToProcess.Id,
                    OutboxStatus.Processing,
                    DateTime.Now);

                await _studyRecordOutboxRepository.CommitAsync();

                _logger.LogDebug($"Processing outbox record with id {nextOutboxItemToProcess.Id} - sending to kafka");

                // Publish to Kafka
                await _studyEventMessagePublisher.Publish(nextOutboxItemToProcess.Payload,
                    cancellationToken);

                _logger.LogDebug($"Processing outbox record with id {nextOutboxItemToProcess.Id} - mark as completed");

                // Mark item as completed
                await _studyRecordOutboxRepository.UpdateOutboxItemStatusAsync(
                    nextOutboxItemToProcess.Id,
                    OutboxStatus.CompletedSuccessfully,
                    null,
                    DateTime.Now);

                await _studyRecordOutboxRepository.CommitAsync();
            }

            _logger.LogInformation($"ProcessAsync completed processing {numberOfOutboxItemsProcessed} records.");
        }
    }
}
