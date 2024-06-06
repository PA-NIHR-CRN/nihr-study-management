﻿using NIHR.StudyManagement.Domain.Models;

namespace NIHR.StudyManagement.Domain.Abstractions
{
    public interface IStudyRegistryRepository
    {
        Task<GovernmentResearchIdentifier> CreateAsync(RegisterStudyRequest request, string griId, CancellationToken cancellationToken = default);

        Task<GovernmentResearchIdentifier> AddStudyToIdentifierAsync(AddStudyToExistingIdentifierRequestWithContext request, CancellationToken cancellationToken = default);

        Task<GovernmentResearchIdentifier> GetAsync(string identifier, CancellationToken cancellationToken = default);
    }
}
