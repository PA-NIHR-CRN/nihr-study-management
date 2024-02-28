namespace NIHR.StudyManagement.Domain.Models
{
    public class RegisterStudyToExistingIdentifierRequest : RegisterStudyRequest
    {
        public string Identifier { get; set; } = "";
    }
}
