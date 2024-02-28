using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Repository.Models
{
    public partial class PersonName : DbEntity
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string Family { get; set; } = null!;
        public string Given { get; set; } = null!;

        public string Email { get; set; } = null!;

        public virtual Person Person { get; set; } = null!;
    }
}
