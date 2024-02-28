using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Repository.Models
{
    public partial class ResearchInitiativeType : DbEntity
    {
        public ResearchInitiativeType()
        {
            ResearchInitiatives = new HashSet<ResearchInitiative>();
        }

        public int Id { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<ResearchInitiative> ResearchInitiatives { get; set; }
    }
}
