using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Repository.Models
{
    public partial class ResearchStudyTeamMember : DbEntity
    {
        public int Id { get; set; }
        public int GriMappingId { get; set; }
        public int PractitionerId { get; set; }
        public int RoleTypeId { get; set; }

        public DateTime EffectiveFrom { get; set; } = DateTime.Now;

        public DateTime? EffectiveTo { get; set; }

        public virtual ResearchStudyEntity GriMapping { get; set; } = null!;
        public virtual RoleTypeEntity PersonRole { get; set; } = null!;
        public virtual Researcher Researcher { get; set; } = null!;
    }
}
