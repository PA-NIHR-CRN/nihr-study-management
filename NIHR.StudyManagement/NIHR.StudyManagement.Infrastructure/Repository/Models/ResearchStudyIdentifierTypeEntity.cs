using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Repository.Models
{
    public partial class ResearchStudyIdentifierTypeEntity : DbEntity
    {
        public ResearchStudyIdentifierTypeEntity()
        {
            Identifiers = new HashSet<ResearchStudyIdentifierEntity>();
        }

        public int Id { get; set; }
        public string Description { get; set; } = null!;

        public virtual ICollection<ResearchStudyIdentifierEntity> Identifiers { get; set; }
    }
}
