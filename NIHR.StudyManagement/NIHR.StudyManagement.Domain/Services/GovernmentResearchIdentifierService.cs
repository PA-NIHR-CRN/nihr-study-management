using Microsoft.Extensions.Options;
using NIHR.StudyManagement.Domain.Abstractions;
using NIHR.StudyManagement.Domain.Configuration;
using NIHR.StudyManagement.Domain.Constants;
using NIHR.StudyManagement.Domain.EnumsAndConstants;
using NIHR.StudyManagement.Domain.Exceptions;
using NIHR.StudyManagement.Domain.Models;
using System.Security.Cryptography;

namespace NIHR.StudyManagement.Domain.Services
{
    public class GovernmentResearchIdentifierService : IGovernmentResearchIdentifierService
    {
        private readonly IStudyRegistryRepository _governmentResearchIdentifierRepository;
        private readonly IStudyEventMessagePublisher _messagePublisher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFhirMapper _fhirMapper;

        private readonly StudyManagementSettings _settings;

        public GovernmentResearchIdentifierService(IStudyRegistryRepository governmentResearchIdentifierRepository,
            IOptions<StudyManagementSettings> settings,
            IStudyEventMessagePublisher messagePublisher,
            IUnitOfWork unitOfWork,
            IFhirMapper fhirMapper)
        {
            this._governmentResearchIdentifierRepository = governmentResearchIdentifierRepository;
            this._settings = settings.Value;
            this._messagePublisher = messagePublisher;
            this._unitOfWork = unitOfWork;
            this._fhirMapper = fhirMapper;

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

            var researchStudy = await _unitOfWork.StudyRegistryRepository.AddStudyToIdentifierAsync(domainRequest, cancellationToken);

            var bundleJson = _fhirMapper.MapToResearchStudyBundleAsJson(researchStudy, request.HttpRequestResponseFhirContext);

            await _unitOfWork.StudyRecordOutboxRepository.AddToOutboxAsync(new AddToOuxboxRequest
            {
                Payload = bundleJson,
                EventType = GrisNsipEventTypes.StudyRegistered,
                SourceSystem = request.ApiSystemName
            },
            cancellationToken);

            await _unitOfWork.CommitAsync();

            return researchStudy;
        }

        private async Task<GovernmentResearchIdentifier> RegisterNewStudyWithNewIdentifierAsync(RegisterStudyRequest request,
            CancellationToken cancellationToken)
        {
            // Generate the GRI
            var gri = GenerateGrisPostcode();

            // Add the GRI as an identifier
            request.Identifiers.Add(new ResearchInitiativeIdentifierItem
            {
                StatusCode = "Active",
                Type = ResearchInitiativeIdentifierTypes.GrisId,
                Value = gri
            });

            // Set API consumer name if not set
            if(string.IsNullOrEmpty(request.ApiSystemName))
            {
                request.ApiSystemName = _settings.DefaultLocalSystemName;
            };

            var researchStudy = await _unitOfWork.StudyRegistryRepository.CreateAsync(request, gri, cancellationToken);

            var bundleJson = _fhirMapper.MapToResearchStudyBundleAsJson(researchStudy, request.HttpRequestResponseFhirContext);

            await _unitOfWork.StudyRecordOutboxRepository.AddToOutboxAsync(new AddToOuxboxRequest
            {
                EventType = GrisNsipEventTypes.StudyRegistered,
                Payload = bundleJson,
                SourceSystem = request.ApiSystemName
            }, cancellationToken);

            await _unitOfWork.CommitAsync();

            return researchStudy;
        }

        public async Task<GovernmentResearchIdentifier> GetAsync(string identifier, CancellationToken cancellationToken = default)
        {
            var existingIdentifier = await _unitOfWork.StudyRegistryRepository.GetAsync(identifier, cancellationToken);

            if (existingIdentifier == null)
            {
                throw new GriNotFoundException();
            }

            return existingIdentifier;
        }

        private async Task<GovernmentResearchIdentifier> GetByIdentifierAsync(string identifier, CancellationToken cancellationToken)
        {
            return await _unitOfWork.StudyRegistryRepository.GetAsync(identifier, cancellationToken);
        }

        /// <summary>
        /// This is a temp implementation for proof of concept to generate a government research identifier.
        /// The implementation simply generates a random sequence of 3 capital letters, 3 numbers and 3 capital letters.
        /// </summary>
        /// <returns></returns>
        private string GenerateGrisPostcode()
        {
            int asciiA = 65;
            int asciiX = 90;
            int ascii1 = 48;
            int ascii9 = 57;
            int maxCharacters = 3;

            var identifier = $"{GetRandomSting(asciiA,asciiX, maxCharacters)}{GetRandomSting(ascii1, ascii9, maxCharacters)}{GetRandomSting(asciiA, asciiX, maxCharacters)}";

            return identifier;
        }

        private static string GetRandomSting(int minAsciiValue, int maxAsciiValue, int numberCharacters)
        {
            var identifier = "";

            for (int i = 0; i < numberCharacters; i++)
            {
                var randomCharacterAscii = RandomNumberGenerator.GetInt32(minAsciiValue, maxAsciiValue);

                var randomCharacter = ((char)randomCharacterAscii).ToString();
                identifier = identifier + randomCharacter;
            }

            return identifier;
        }
    }
}
