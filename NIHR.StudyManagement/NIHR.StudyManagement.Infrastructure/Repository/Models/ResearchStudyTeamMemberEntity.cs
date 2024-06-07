using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Repository.Models
{
    public partial class ResearchStudyTeamMemberEntity : DbEntity
    {
        public int Id { get; set; }
        public int ResearchStudyId { get; set; }
        public int PractitionerId { get; set; }
        public int RoleTypeId { get; set; }

        public int? OrganizationId { get; set; }

        public DateTime EffectiveFrom { get; set; } = DateTime.Now;

        public DateTime? EffectiveTo { get; set; }

        public virtual ResearchStudyEntity ResearchStudy { get; set; } = null!;

        public virtual RoleTypeEntity PersonRole { get; set; } = null!;

        public virtual PractitionerEntity Practitioner { get; set; } = null!;

        public virtual OrganisationEntity? Organitation { get; set; }
    }
}
