
namespace NIHR.StudyManagement.Domain.Models
{
    public class Organisation
    {
        public string Code { get; private set; }

        public string Description { get; private set; }

        public Organisation(string code, string description)
        {
            Code = code;
            Description = description;
        }
    }
}
