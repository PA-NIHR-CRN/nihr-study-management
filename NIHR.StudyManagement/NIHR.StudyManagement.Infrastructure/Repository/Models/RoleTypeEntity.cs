using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Repository.Models
{
    public partial class RoleTypeEntity : DbEntity
    {
        public RoleTypeEntity()
        {
            ResearchStudyTeamMembers = new HashSet<ResearchStudyTeamMemberEntity>();
        }

        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<ResearchStudyTeamMemberEntity> ResearchStudyTeamMembers { get; set; }
    }

    public partial class OrganisationEntity : DbEntity
    {
        public int Id { get; set; }

        public string Code { get; set; } = null!;

        public string Description { get; set; } = null!;

        public virtual ICollection<ResearchStudyTeamMemberEntity> ResearchStudyTeamMembers { get; set; }

        public OrganisationEntity()
        {
            ResearchStudyTeamMembers = new List<ResearchStudyTeamMemberEntity>();
        }
    }
}
