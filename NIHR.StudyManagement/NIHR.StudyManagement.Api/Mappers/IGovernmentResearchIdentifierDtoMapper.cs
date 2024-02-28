using NIHR.StudyManagement.Api.Models.Dto;
using NIHR.StudyManagement.Domain.Models;

namespace NIHR.StudyManagement.Api.Mappers
{
    public interface IGovernmentResearchIdentifierDtoMapper
    {
        RegisterStudyRequest Map(RegisterStudyRequestDto requestDto);

        RegisterStudyToExistingIdentifierRequest Map(RegisterStudyRequestDto requestDto, string identifier);

        GovernmentResearchIdentifierDto Map(GovernmentResearchIdentifier governmentResearchIdentifier);
    }
}
