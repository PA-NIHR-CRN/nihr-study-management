using System.ComponentModel.DataAnnotations;

namespace NIHR.StudyManagement.Api.Models.Dto
{
    /// <summary>
    /// Object to represent a person
    /// </summary>
    public class PersonDto
    {
        /// <summary>
        /// First name
        /// </summary>
        [Required]
        public string Firstname { get; set; } = "";

        /// <summary>
        /// Last name
        /// </summary>

        [Required]
        public string Lastname { get; set; } = "";
    }
}
