using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Repository.Models
{
    public partial class SourceSystemEntity : DbEntity
    {
        public SourceSystemEntity()
        {
            ResearchStudyIdentifiers = new HashSet<ResearchStudyIdentifierEntity>();
            ResearchStudies = new HashSet<ResearchStudyEntity>();
        }

        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Description { get; set; } = null!;

        public virtual ICollection<ResearchStudyIdentifierEntity> ResearchStudyIdentifiers { get; set; }
        public virtual ICollection<ResearchStudyEntity> ResearchStudies { get; set; }
    }
}
