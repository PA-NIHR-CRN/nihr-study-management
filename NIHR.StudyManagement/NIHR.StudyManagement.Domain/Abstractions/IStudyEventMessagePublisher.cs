
namespace NIHR.StudyManagement.Domain.Abstractions
{
    public interface IStudyEventMessagePublisher
    {
        Task PrepareAndPublishAsync(string eventType,
            string sourceSystemName,
            string payload,
            CancellationToken cancellationToken);

        Task Publish(string payload, CancellationToken cancellationToken);
    }
}
