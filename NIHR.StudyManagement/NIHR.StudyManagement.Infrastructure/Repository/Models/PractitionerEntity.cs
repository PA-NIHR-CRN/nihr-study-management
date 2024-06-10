using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Repository.Models
{
    public partial class PractitionerEntity : DbEntity
    {
        public PractitionerEntity()
        {
            ResearchStudyTeamMembers = new HashSet<ResearchStudyTeamMemberEntity>();
        }

        public int Id { get; set; }
        public int PersonId { get; set; }

        public virtual PersonEntity Person { get; set; } = null!;
        public virtual ICollection<ResearchStudyTeamMemberEntity> ResearchStudyTeamMembers { get; set; }
    }
}
