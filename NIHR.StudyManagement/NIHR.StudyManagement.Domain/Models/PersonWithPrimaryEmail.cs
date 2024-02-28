namespace NIHR.StudyManagement.Domain.Models
{
    public class PersonWithPrimaryEmail : Person
    {
        public Email Email { get; set; }

        public PersonWithPrimaryEmail()
        {
            Email = new Email();
        }
    }
}
