using NIHR.StudyManagement.Domain.Constants;

namespace NIHR.StudyManagement.Infrastructure.Repository.Models
{
    public partial class StudyRecordOutboxEntity : DbEntity
    {
        public int Id { get; set; }

        public string Payload { get; set; }

        public string SourceSystem { get; set; }

        public string EventType { get; set; }

        public DateTime? ProcessingStartDate { get; set; }

        public DateTime? ProcessingCompletedDate { get; set; }

        public OutboxStatus Status { get; set; }

        public StudyRecordOutboxEntity()
        {
            Payload = "";
            SourceSystem = "";
            EventType = "";
            Status = OutboxStatus.Created;
        }
    }
}
