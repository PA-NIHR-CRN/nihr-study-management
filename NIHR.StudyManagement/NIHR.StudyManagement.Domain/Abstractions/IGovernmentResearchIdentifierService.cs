using NIHR.StudyManagement.Domain.Models;

namespace NIHR.StudyManagement.Domain.Abstractions
{
    public interface IGovernmentResearchIdentifierService
    {
        Task<GovernmentResearchIdentifier> RegisterStudyAsync(RegisterStudyRequest request, CancellationToken cancellationToken = default);

        Task<GovernmentResearchIdentifier> RegisterStudyAsync(RegisterStudyToExistingIdentifierRequest request, CancellationToken cancellationToken = default);

        Task<GovernmentResearchIdentifier> GetAsync(string identifier, CancellationToken cancellationToken = default);
    }
}
