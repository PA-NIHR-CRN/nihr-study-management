namespace NIHR.StudyManagement.Domain.Models
{
    public class Person
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public Person()
        {
            Firstname = "";
            Lastname = "";
        }
    }
}
