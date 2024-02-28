
namespace NIHR.StudyManagement.Domain.Configuration
{
    public class StudyManagementSettings
    {
        public string DefaultRoleName {get;set;}

        public string DefaultLocalSystemName {get;set;}

        public StudyManagementSettings()
        {
            DefaultRoleName = "";
            DefaultLocalSystemName = "";
        }
    }
}
