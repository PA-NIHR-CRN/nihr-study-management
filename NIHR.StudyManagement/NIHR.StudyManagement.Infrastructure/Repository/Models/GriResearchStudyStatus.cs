namespace NIHR.StudyManagement.Infrastructure.Repository.Models
{
    public partial class GriResearchStudyStatus : DbEntity
    {
        public int Id { get; set; }

        public int GriMappingId { get; set; }

        public string Code { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public virtual GriMapping GriMapping { get; set; } = null!;
    }
}
