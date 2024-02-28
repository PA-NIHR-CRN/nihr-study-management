namespace NIHR.StudyManagement.Domain.Models
{
    public class RegisterStudyRequestWithContext : RegisterStudyRequest
    {
        public string LocalSystemName {get;set;}

        public string RoleName { get; set; }

        public string Identifier { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public RegisterStudyRequestWithContext()
        {
            Identifier = "";
            LocalSystemName = "";
            RoleName = "";
            EffectiveFrom = DateTime.Now;
        }
    }
}
