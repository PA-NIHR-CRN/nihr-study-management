namespace NIHR.StudyManagement.Domain.Abstractions
{
    public interface IUnitOfWork
    {
        IStudyRegistryRepository StudyRegistryRepository { get; }

        IStudyRecordOutboxRepository StudyRecordOutboxRepository { get; }

        Task CommitAsync();
        void Dispose();
    }
}