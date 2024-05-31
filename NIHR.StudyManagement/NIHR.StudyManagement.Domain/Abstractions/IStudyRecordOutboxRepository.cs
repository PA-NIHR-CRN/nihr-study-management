using NIHR.StudyManagement.Domain.Constants;
using NIHR.StudyManagement.Domain.Models;

namespace NIHR.StudyManagement.Domain.Abstractions
{

    public interface IStudyRecordOutboxRepository
    {
        Task AddToOutboxAsync(AddToOuxboxRequest request, CancellationToken cancellationToken);

        StudyRecordOutboxItem? GetNextUnprocessedOutboxItem();

        Task UpdateOutboxItemStatusAsync(int id,
            OutboxStatus status,
            DateTime? processingStartedTime = null,
            DateTime? processingCompletedTime = null);

        Task CommitAsync();
    }
}
