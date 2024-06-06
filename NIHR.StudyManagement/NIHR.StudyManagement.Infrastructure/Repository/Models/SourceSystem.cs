using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Repository.Models
{
    public partial class SourceSystem : DbEntity
    {
        public SourceSystem()
        {
            GriMappings = new HashSet<ResearchStudyIdentifierEntity>();
            GriResearchStudies = new HashSet<ResearchStudyEntity>();
        }

        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Description { get; set; } = null!;

        public virtual ICollection<ResearchStudyIdentifierEntity> GriMappings { get; set; }
        public virtual ICollection<ResearchStudyEntity> GriResearchStudies { get; set; }
    }
}
