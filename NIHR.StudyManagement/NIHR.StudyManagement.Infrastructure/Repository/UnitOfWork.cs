using Microsoft.EntityFrameworkCore;
using NIHR.StudyManagement.Domain.Abstractions;

namespace NIHR.StudyManagement.Infrastructure.Repository
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly StudyRegistryContext _context;

        public IStudyRegistryRepository StudyRegistryRepository { get; private set; }
        public IStudyRecordOutboxRepository StudyRecordOutboxRepository { get;private set; }

        public UnitOfWork(StudyRegistryContext dbContext, INsipGrisMessageHelper nsipGrisMessageHelper)
        {
            _context = dbContext;
            StudyRegistryRepository = new StudyRegistryRepository(dbContext);
            StudyRecordOutboxRepository = new StudyRecordOutboxRepository(dbContext, nsipGrisMessageHelper);
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
