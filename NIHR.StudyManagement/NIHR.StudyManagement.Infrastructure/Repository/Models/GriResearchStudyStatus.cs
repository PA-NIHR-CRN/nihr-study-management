namespace NIHR.StudyManagement.Infrastructure.Repository.Models
{
    public partial class GriResearchStudyStatus : DbEntity
    {
        public int Id { get; set; }

        public int ResearchStudyIdentifierId { get; set; }

        public string Code { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public virtual GriMapping ResearchStudyIdentifier { get; set; } = null!;
    }
}
