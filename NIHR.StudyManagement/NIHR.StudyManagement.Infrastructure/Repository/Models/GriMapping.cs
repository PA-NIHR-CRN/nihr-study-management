using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Repository.Models
{
    public abstract class DbEntity
    {
        public DateTime Created { get; set; } = DateTime.Now;
    }

    public partial class GriMapping : DbEntity
    {
        public int Id { get; set; }
        public int GriResearchStudyId { get; set; }
        public int SourceSystemId { get; set; }
        public int ResearchInitiativeIdentifierId { get; set; }

        public virtual GriResearchStudy GriResearchStudy { get; set; } = null!;
        public virtual ResearchInitiativeIdentifier ResearchInitiativeIdentifier { get; set; } = null!;
        public virtual SourceSystem SourceSystem { get; set; } = null!;

        public virtual ICollection<GriResearchStudyStatus> GriResearchStudyStatuses { get; set; } = null!;
    }
}
