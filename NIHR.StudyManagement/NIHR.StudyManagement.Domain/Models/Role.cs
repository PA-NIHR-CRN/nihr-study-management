namespace NIHR.StudyManagement.Domain.Models
{
    public class Role
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public Role()
        {
            Code = "";
            Description = "";
        }
    }
}
