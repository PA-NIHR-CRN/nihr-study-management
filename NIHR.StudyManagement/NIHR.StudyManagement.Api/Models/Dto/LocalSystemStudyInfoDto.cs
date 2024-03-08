using System.ComponentModel.DataAnnotations;

namespace NIHR.StudyManagement.Api.Models.Dto
{
    /// <summary>
    /// Object to represent the local system project/study info.
    /// </summary>
    public class LocalSystemStudyInfoDto
    {
        /// <summary>
        /// The project/study unique id within the local system.
        /// </summary>
        [Required]
        public string ProjectId { get; set; } = "";

        /// <summary>
        /// The project/study short title.
        /// </summary>
        [Required]
        public string ShortTitle { get; set; } = "";

        /// <summary>
        /// The status at the time the study is being created within the source system.
        /// </summary>
        [Required]
        public string Status { get; set; } = "";
    }
}
