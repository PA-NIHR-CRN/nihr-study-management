using System.ComponentModel.DataAnnotations;

namespace NIHR.StudyManagement.Api.Models.Dto
{
    /// <summary>
    /// Object to represent a person with an email and a role.
    /// </summary>
    public class RegisterStudyRequestPersonRoleDto
    {
        /// <summary>
        /// Person first name
        /// </summary>
        [Required]
        public string Firstname { get; set; } = "";

        /// <summary>
        /// Person last name
        /// </summary>
        [Required]
        public string Lastname { get; set; } = "";

        /// <summary>
        /// Primary email contact.
        /// </summary>
        [Required]
        public string PrimaryEmail { get; set; } = "";

        /// <summary>
        /// The role of the person. Currently only supported value is CHIEF_INVESTIGATOR
        /// </summary>
        [Required]
        public string Role { get; set; } = "";

    }

    /// <summary>
    /// Object to represent a request to register a new study where no Government Research Identifier exists.
    /// </summary>
    public class RegisterStudyRequestDto
    {
        /// <summary>
        /// A collection of team members.
        /// </summary>
        [Required]
        public List<RegisterStudyRequestPersonRoleDto> TeamMembers { get; set; } = new List<RegisterStudyRequestPersonRoleDto>();

        /// <summary>
        /// Information of the study in the local/source system.
        /// </summary>
        [Required]
        public LocalSystemStudyInfoDto LocalStudy { get; set; } = new LocalSystemStudyInfoDto();

        /// <summary>
        /// The protocol ID.
        /// </summary>
        [Required]
        public string ProtocolId { get; set; } = "";
    }

    /// <summary>
    /// Object which represents a request to register a new local study record against a known Government
    /// Research Identifier.
    /// </summary>
    public class RegisterStudyWithKnownGriDto
    {
        /// <summary>
        /// An object representing the chief investigator. Mandatory
        /// </summary>
        /// <remarks>
        /// This is a remark
        /// </remarks>
        [Required]
        public PersonWithEmailDto ChiefInvestigator { get; set; } = new PersonWithEmailDto();

        /// <summary>
        /// Information of the study in the local/source system.
        /// </summary>
        [Required]
        public LocalSystemStudyInfoDto LocalStudy { get; set; } = new LocalSystemStudyInfoDto();

        /// <summary>
        /// The existing Government Research Identifier
        /// </summary>
        [Required]
        public string Gri { get; set; } = "";
    }
}
