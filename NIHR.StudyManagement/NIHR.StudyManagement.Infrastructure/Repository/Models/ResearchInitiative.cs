using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Repository.Models
{
    public partial class ResearchInitiative : DbEntity
    {
        public ResearchInitiative()
        {
            GriResearchStudies = new HashSet<GriResearchStudy>();
        }

        public int Id { get; set; }
        public int? ResearchInitiativeTypeId { get; set; }

        public virtual ResearchInitiativeType? ResearchInitiativeType { get; set; }
        public virtual ICollection<GriResearchStudy> GriResearchStudies { get; set; }
    }
}
