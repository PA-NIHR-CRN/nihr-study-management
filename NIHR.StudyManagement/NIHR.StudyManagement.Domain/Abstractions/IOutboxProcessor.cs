namespace NIHR.StudyManagement.Domain.Abstractions
{
    public interface IOutboxProcessor
    {
        Task ProcessAsync(CancellationToken cancellationToken);
    }
}
