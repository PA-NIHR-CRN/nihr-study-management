using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NIHR.StudyManagement.Domain.Abstractions;
using NIHR.StudyManagement.Domain.Configuration;
using NIHR.StudyManagement.Domain.Constants;
using NIHR.StudyManagement.Domain.EnumsAndConstants;
using NIHR.StudyManagement.Domain.Exceptions;
using NIHR.StudyManagement.Domain.Models;

namespace NIHR.StudyManagement.Domain.Services
{
    public class GovernmentResearchIdentifierService : IGovernmentResearchIdentifierService
    {
        private readonly IStudyRegistryRepository _governmentResearchIdentifierRepository;
        private readonly IStudyEventMessagePublisher _messagePublisher;
        private readonly IRandomNumberGenerator _randomNumberGenerator;
        private readonly ILogger<GovernmentResearchIdentifierService> _logger;

        private readonly StudyManagementSettings _settings;

        public GovernmentResearchIdentifierService(IStudyRegistryRepository governmentResearchIdentifierRepository,
            IOptions<StudyManagementSettings> settings,
            IStudyEventMessagePublisher messagePublisher,
            IRandomNumberGenerator randomNumberGenerator,
            ILogger<GovernmentResearchIdentifierService> logger)
        {
            this._governmentResearchIdentifierRepository = governmentResearchIdentifierRepository;
            this._settings = settings.Value;
            this._messagePublisher = messagePublisher;
            this._randomNumberGenerator = randomNumberGenerator;
            this._logger = logger;

            if (string.IsNullOrEmpty(this._settings.DefaultRoleName)) throw new ArgumentNullException(nameof(_settings.DefaultRoleName));

            if (string.IsNullOrEmpty(this._settings.DefaultLocalSystemName)) throw new ArgumentNullException(nameof(_settings.DefaultLocalSystemName));
        }

        public async Task<GovernmentResearchIdentifier> RegisterStudyAsync(RegisterStudyRequest request,
            CancellationToken cancellationToken = default)
        {
            return await RegisterNewStudyWithNewIdentifierAsync(request, cancellationToken);
        }

        public async Task<GovernmentResearchIdentifier> RegisterStudyAsync(RegisterStudyToExistingIdentifierRequest request,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(request.Identifier)) throw new ArgumentNullException(nameof(request.Identifier));

            return await AddNewStudyToExistingIdentifierAsync(request, cancellationToken);
        }

        private async Task<GovernmentResearchIdentifier> AddNewStudyToExistingIdentifierAsync(RegisterStudyToExistingIdentifierRequest request,
            CancellationToken cancellationToken)
        {
            if (request == null || string.IsNullOrEmpty(request.Identifier)) throw new ArgumentNullException(nameof(request));

            // Verify that the GRI exists
            var existingIdentifier = await GetByIdentifierAsync(request.Identifier, cancellationToken);

            if(existingIdentifier == null)
            {
                throw new GriNotFoundException($"Could not find an existing GRI with the specified identifier '{request.Identifier}'");
            }

            // Ensure no existing link exists for the same local system record
            if(existingIdentifier.LinkedSystemIdentifiers.Any(linked => linked.Identifier == request.ProjectId
                && linked.SystemName == _settings.DefaultLocalSystemName
                // Todo: need to add effective from/to dates on this record for evaluation here.
                ))
            {
                throw new GriAlreadyExistsException($"A local system identifier already exists for GRI '{request.Identifier}' and '{_settings.DefaultLocalSystemName}'");
            }

            var domainRequest = new AddStudyToExistingIdentifierRequestWithContext
            {
                RequestContext = request
            };

            // Create a list of identifiers to add
            foreach (var requestLinkedIdentifier in request.Identifiers)
            {
                if(requestLinkedIdentifier.Type == ResearchInitiativeIdentifierTypes.Bundle
                    || requestLinkedIdentifier.Type == ResearchInitiativeIdentifierTypes.GrisId)
                {
                    continue;
                }

                // Check against existing identifiers
                var isNewIdentifier = true;

                foreach (var existingLinkedIdentifier in existingIdentifier.LinkedSystemIdentifiers)
                {
                    if(existingLinkedIdentifier.IdentifierType.Equals(requestLinkedIdentifier.Type, StringComparison.Ordinal)
                        && existingLinkedIdentifier.Identifier.Equals(requestLinkedIdentifier.Value, StringComparison.OrdinalIgnoreCase))
                    {
                        isNewIdentifier = false;
                        break;
                    }
                }

                if(isNewIdentifier)
                {
                    domainRequest.LinkedSystemIdentifiersToAdd.Add(new LinkedSystemIdentifier {
                        CreatedAt = DateTime.Now,
                        Identifier = requestLinkedIdentifier.Value,
                        IdentifierType = requestLinkedIdentifier.Type,
                        SystemName = request.ApiSystemName
                    });
                }
            }

            var researchStudy = await _governmentResearchIdentifierRepository.AddStudyToIdentifierAsync(domainRequest, cancellationToken);

            await _messagePublisher.PublishAsync(GrisNsipEventTypes.StudyRegistered, request.ApiSystemName, researchStudy, cancellationToken);

            return researchStudy;
        }

        private RegisterStudyRequestWithContext Map(RegisterStudyRequest request, string identifier)
        {
            return new RegisterStudyRequestWithContext
            {
                ChiefInvestigator = request.ChiefInvestigator,
                ApiSystemName = request.ApiSystemName,
                ProjectId = request.ProjectId,
                Identifier = identifier,
                Identifiers = request.Identifiers,
                Sponsor = request.Sponsor,
                ShortTitle = request.ShortTitle,
                RoleName = _settings.DefaultRoleName,
                ProtocolId = request.ProtocolId,
                StatusCode = request.StatusCode
            };
        }

        private async Task<GovernmentResearchIdentifier> RegisterNewStudyWithNewIdentifierAsync(RegisterStudyRequest request,
            CancellationToken cancellationToken)
        {
            // Generate the GRI
            var gri = await GenerateNewGrisIdAsync(cancellationToken);

            var domainRequest =  Map(request, gri.DisplayValue);

            var researchStudy = await _governmentResearchIdentifierRepository.CreateAsync(domainRequest, cancellationToken);

            await _messagePublisher.PublishAsync(GrisNsipEventTypes.StudyRegistered, request.ApiSystemName, researchStudy, cancellationToken);

            return researchStudy;
        }

        private async Task<GrisId> GenerateNewGrisIdAsync(CancellationToken cancellationToken)
        {
            // Allow one failure/duplicate
            for(int retries = 3;retries > 0;retries--)
            {
                var grisId = GrisId.GenerateNewGrisId(_randomNumberGenerator);

                // Check if ID already exists
                if(await _governmentResearchIdentifierRepository.DoesGrisExist(grisId.DisplayValue, cancellationToken) )
                {
                    // Warn
                    _logger.LogWarning($"Randomly generated sequence {grisId.DisplayValue} already exists. Retries left {retries - 1}");
                    continue;
                }

                return grisId;
            }

            _logger.LogError($"Could not generate GRIS ID. Exceeded max attempts.");

            throw new Exception("Could not generate GRIS ID");
        }

        public async Task<GovernmentResearchIdentifier> GetAsync(string identifier, CancellationToken cancellationToken = default)
        {
            var existingIdentifier = await _governmentResearchIdentifierRepository.GetAsync(identifier, cancellationToken);

            if (existingIdentifier == null)
            {
                throw new GriNotFoundException();
            }

            return existingIdentifier;
        }

        private async Task<GovernmentResearchIdentifier> GetByIdentifierAsync(string identifier, CancellationToken cancellationToken)
        {
            return await _governmentResearchIdentifierRepository.GetAsync(identifier, cancellationToken);
        }
    }
}
