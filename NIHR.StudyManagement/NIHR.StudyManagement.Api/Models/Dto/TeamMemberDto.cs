namespace NIHR.StudyManagement.Api.Models.Dto
{
    /// <summary>
    /// Object representing a team member within the context of a government research identifier.
    /// </summary>
    public class TeamMemberDto
    {
        /// <summary>
        /// Object representing the team member 
        /// </summary>
        public PersonWithEmailDto Person { get; set; } = new PersonWithEmailDto();

        /// <summary>
        /// Object representing the role of the given person, associated with the given government research identifier.
        /// </summary>
        public RoleDto Role { get; set; } = new RoleDto();

        /// <summary>
        /// The effective start date of the role for the person associated with the government research identifier.
        /// </summary>
        public DateTime EffectiveFrom { get; set; } = DateTime.Now;

        /// <summary>
        /// The effective end date of the role for the person associated with the government research identifier.
        /// Null implies no end date set and role is potentially active.
        /// </summary>
        public DateTime? EffectiveTo { get; set; }
    }
}