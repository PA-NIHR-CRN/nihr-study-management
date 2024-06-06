using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Repository.Models
{   
    public partial class ResearchStudyEntity : DbEntity
    {
        public ResearchStudyEntity()
        {
            ResearchStudyIdentifiers = new HashSet<ResearchStudyIdentifierEntity>();
            ResearchStudyTeamMembers = new HashSet<ResearchStudyTeamMember>();
        }

        public int Id { get; set; }
        public string Gri { get; set; } = null!;
        public string ShortTitle { get; set; } = null!;
        public int RequestSourceSystemId { get; set; }

        public virtual SourceSystem RequestSourceSystem { get; set; } = null!;
        public virtual ICollection<ResearchStudyIdentifierEntity> ResearchStudyIdentifiers { get; set; }
        public virtual ICollection<ResearchStudyTeamMember> ResearchStudyTeamMembers { get; set; }
    }
}
