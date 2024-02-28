namespace NIHR.StudyManagement.Domain.Models
{
    public class TeamMember
    {
        public PersonWithPrimaryEmail Person { get; set; }

        public Role Role { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public TeamMember()
        {
            Person = new PersonWithPrimaryEmail();
            Role = new Role();
            EffectiveFrom = DateTime.Now;
        }
    }
}
