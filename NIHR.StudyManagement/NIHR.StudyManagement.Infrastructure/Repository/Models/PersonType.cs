using System;
using System.Collections.Generic;

namespace NIHR.StudyManagement.Infrastructure.Repository.Models
{
    public partial class PersonType : DbEntity
    {
        public PersonType()
        {
            People = new HashSet<Person>();
        }

        public int Id { get; set; }
        public string Description { get; set; } = null!;

        public virtual ICollection<Person> People { get; set; }
    }
}
