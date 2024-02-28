namespace NIHR.StudyManagement.Domain.Models
{
    public class Role
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Role()
        {
            Name = "";
            Description = "";
        }
    }
}
