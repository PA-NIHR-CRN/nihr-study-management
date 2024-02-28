
namespace NIHR.StudyManagement.Domain.Models
{
    public abstract class DomainEntity
    {
        public DateTime Created { get; set; }

        public DomainEntity()
        {
            Created = DateTime.Now;
        }
    }
}
