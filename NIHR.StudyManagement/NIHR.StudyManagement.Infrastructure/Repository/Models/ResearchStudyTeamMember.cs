using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Repository.Models
{
    public partial class ResearchStudyTeamMember : DbEntity
    {
        public int Id { get; set; }
        public int GriMappingId { get; set; }
        public int ResearcherId { get; set; }
        public int PersonRoleId { get; set; }

        public DateTime EffectiveFrom { get; set; } = DateTime.Now;

        public DateTime? EffectiveTo { get; set; }

        public virtual GriResearchStudy GriMapping { get; set; } = null!;
        public virtual PersonRole PersonRole { get; set; } = null!;
        public virtual Researcher Researcher { get; set; } = null!;
    }
}
