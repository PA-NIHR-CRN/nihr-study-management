using NIHR.StudyManagement.Domain.Abstractions;
using NIHR.StudyManagement.Domain.Constants;
using NIHR.StudyManagement.Domain.Models;
using System.Text.Json;

namespace NIHR.StudyManagement.Infrastructure.Repository
{
    public class StudyRecordOutboxRepository : IStudyRecordOutboxRepository
    {
        private readonly StudyRegistryContext _context;
        private readonly INsipGrisMessageHelper _nsipGrisMessageHelper;

        public StudyRecordOutboxRepository(StudyRegistryContext context,
            INsipGrisMessageHelper nsipGrisMessageHelper)
        {
            _context = context;
            _nsipGrisMessageHelper = nsipGrisMessageHelper;
        }

        public async Task AddToOutboxAsync(AddToOuxboxRequest request, CancellationToken cancellationToken)
        {
            // Wrap the payload json in an NSIP message
            var nsipMessage = _nsipGrisMessageHelper.Prepare(request.EventType, request.SourceSystem, request.Payload);

            // Serialize the NSIP message using standard serialization options.
            var nsipMessageAsPayload = JsonSerializer.Serialize(nsipMessage);

            await _context.StudyRecordOutboxEntries.AddAsync(new Models.StudyRecordOutboxEntity
            {
                EventType = request.EventType,
                SourceSystem = request.SourceSystem,
                Payload = nsipMessageAsPayload
            });
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public StudyRecordOutboxItem? GetNextUnprocessedOutboxItem()
        {
            var nextOutboxItem = _context.StudyRecordOutboxEntries
                 .FirstOrDefault(outbox => outbox.Status == OutboxStatus.Created);

            if(nextOutboxItem == null)
            {
                return null;
            }

            return new StudyRecordOutboxItem(nextOutboxItem.Id,
                nextOutboxItem.Payload,
                nextOutboxItem.SourceSystem,
                nextOutboxItem.EventType,
                nextOutboxItem.ProcessingStartDate,
                nextOutboxItem.ProcessingCompletedDate,
                nextOutboxItem.Status);
        }

        public async Task UpdateOutboxItemStatusAsync(int id, OutboxStatus status, DateTime? processingStartedTime = null, DateTime? processingCompletedTime = null)
        {
            var recordToUpdate = await _context
                .StudyRecordOutboxEntries
                .FindAsync(id);

            if(recordToUpdate == null)
            {
                throw new KeyNotFoundException($"Could not find outbox entry for id {id}");
            }

            recordToUpdate.Status = status;

            recordToUpdate.ProcessingStartDate = processingStartedTime.HasValue
                ? processingStartedTime.Value
                : recordToUpdate.ProcessingStartDate;

            recordToUpdate.ProcessingCompletedDate = processingCompletedTime.HasValue
                ? processingCompletedTime.Value
                : recordToUpdate.ProcessingCompletedDate;
        }
    }
}
