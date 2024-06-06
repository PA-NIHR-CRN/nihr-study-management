using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Repository.Models
{   
    public partial class ResearchStudyEntity : DbEntity
    {
        public ResearchStudyEntity()
        {
            ResearchStudyIdentifiers = new HashSet<ResearchStudyIdentifierEntity>();
            ResearchStudyTeamMembers = new HashSet<ResearchStudyTeamMemberEntity>();
        }

        public int Id { get; set; }
        public string Gri { get; set; } = null!;
        public string ShortTitle { get; set; } = null!;
        public int RequestSourceSystemId { get; set; }

        public virtual SourceSystemEntity RequestSourceSystem { get; set; } = null!;
        public virtual ICollection<ResearchStudyIdentifierEntity> ResearchStudyIdentifiers { get; set; }
        public virtual ICollection<ResearchStudyTeamMemberEntity> ResearchStudyTeamMembers { get; set; }
    }
}
