using System.ComponentModel.DataAnnotations;

namespace NIHR.StudyManagement.Api.Models.Dto
{
    /// <summary>
    /// Object to represent a person with an email address
    /// </summary>
    public class PersonWithEmailDto : PersonDto
    {
        /// <summary>
        /// Primary email address of person.
        /// </summary>
        [Required]
        public string Email { get; set; } = "";
    }
}
