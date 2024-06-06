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
        //public int IdentifierId { get; set; }

        public string Value { get; set; } = null!;

        public int IdentifierTypeId { get; set; }

        public virtual ResearchInitiativeIdentifierType IdentifierType { get; set; } = null!;

        public virtual GriResearchStudy GriResearchStudy { get; set; } = null!;
        //public virtual ResearchInitiativeIdentifier Identifier { get; set; } = null!;
        public virtual SourceSystem SourceSystem { get; set; } = null!;

        public virtual ICollection<GriResearchStudyStatus> IdentifierStatuses { get; set; } = null!;
    }
}
