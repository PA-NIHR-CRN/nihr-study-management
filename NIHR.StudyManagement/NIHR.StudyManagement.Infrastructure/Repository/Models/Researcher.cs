using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Repository.Models
{
    public partial class Researcher : DbEntity
    {
        public Researcher()
        {
            ResearchStudyTeamMembers = new HashSet<ResearchStudyTeamMember>();
        }

        public int Id { get; set; }
        public int PersonId { get; set; }

        public virtual PersonEntity Person { get; set; } = null!;
        public virtual ICollection<ResearchStudyTeamMember> ResearchStudyTeamMembers { get; set; }
    }
}
