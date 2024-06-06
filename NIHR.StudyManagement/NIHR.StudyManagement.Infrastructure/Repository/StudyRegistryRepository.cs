using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NIHR.StudyManagement.Domain.Abstractions;
using NIHR.StudyManagement.Domain.Configuration;
using NIHR.StudyManagement.Domain.EnumsAndConstants;
using NIHR.StudyManagement.Domain.Exceptions;
using NIHR.StudyManagement.Domain.Models;
using NIHR.StudyManagement.Infrastructure.Repository.EnumsAndConstants;
using NIHR.StudyManagement.Infrastructure.Repository.Models;
using System;

using PersonDb = NIHR.StudyManagement.Infrastructure.Repository.Models.Person;

namespace NIHR.StudyManagement.Infrastructure.Repository
{
    public class StudyRegistryRepository : IStudyRegistryRepository
    {
        private readonly StudyRegistryContext _context;

        public StudyRegistryRepository(StudyRegistryContext context)
        {
            _context = context;
        }

        public async Task<GovernmentResearchIdentifier> CreateAsync(RegisterStudyRequestWithContext request,
            CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            };


            var edgeSystem = await GetSourceSystemAsync(request.ApiSystemName, cancellationToken) ?? throw new EntityNotFoundException(nameof(SourceSystem));

            var personRoleCI = await _context.PersonRoles.FirstOrDefaultAsync(x => x.Code == PersonRoles.ChiefInvestigator, cancellationToken) ?? throw new EntityNotFoundException(nameof(PersonRole));

            var researchInitiativeIdentifierTypes = new Dictionary<string, ResearchInitiativeIdentifierType>();

            foreach (var x in _context.ResearchInitiativeIdentifierTypes.Select(x => x))
            {
                researchInitiativeIdentifierTypes[x.Description] = x;
            }

            var researchStudy = new GriResearchStudy
            {
                ShortTitle = request.ShortTitle,
                Gri = request.Identifier ?? "",
                RequestSourceSystem = edgeSystem
            };

            GriMapping mainGriMappingForStatus = null;

            foreach (var identifier in request.Identifiers)
            {
                var griMapping = new GriMapping
                {
                    GriResearchStudy = researchStudy,
                    Value = identifier.Value,
                    IdentifierType = researchInitiativeIdentifierTypes[identifier.Type],
                    SourceSystem = edgeSystem
                };

                await _context.AddAsync(griMapping, cancellationToken);

                if (identifier.Type == ResearchInitiativeIdentifierTypes.Project)
                {
                    mainGriMappingForStatus = griMapping;
                }
            }

            if(mainGriMappingForStatus != null)
            {
                var griResearchStudyStatus = new GriResearchStudyStatus
                {
                    Code = request.StatusCode,
                    ResearchStudyIdentifier = mainGriMappingForStatus,
                    FromDate = DateTime.Now
                };

                await _context.AddAsync(griResearchStudyStatus, cancellationToken);
            }

            var chiefInvestigator = await GetPersonAsync(request.ChiefInvestigator, cancellationToken) ?? new PersonDb
            {
                PersonNames = new PersonName[] { new PersonName {
                        Given =request.ChiefInvestigator.Firstname,
                        Family = request.ChiefInvestigator.Lastname,
                        Email = request.ChiefInvestigator.Email.Address
                    } }
            };

            var teamMember = new ResearchStudyTeamMember
            {
                GriMapping = researchStudy,
                Researcher = new Researcher
                {
                    Person = chiefInvestigator
                },
                PersonRole = personRoleCI
            };


            await _context.AddAsync(teamMember, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return await GetAsync(request.Identifier);
        }

        public async Task<GovernmentResearchIdentifier> AddStudyToIdentifierAsync(AddStudyToExistingIdentifierRequestWithContext request,
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var griResearchStudy = await GetGriResearchStudyByIdentifierAsync(request.RequestContext.Identifier, cancellationToken) ?? throw new GriNotFoundException();

            var sourceSystem = await GetSourceSystemAsync(request.RequestContext.ApiSystemName, cancellationToken) ?? throw new EntityNotFoundException(nameof(SourceSystem));

            var researchInitiativeIdentifierTypes = new Dictionary<string, ResearchInitiativeIdentifierType>();

            foreach (var x in _context.ResearchInitiativeIdentifierTypes.Select(x => x))
            {
                researchInitiativeIdentifierTypes[x.Description] = x;
            }

            foreach (var identifierToAdd in request.LinkedSystemIdentifiersToAdd)
            {
                var identifierType = researchInitiativeIdentifierTypes[identifierToAdd.IdentifierType];

                var griMapping = new GriMapping
                {
                    GriResearchStudy = griResearchStudy,
                    Value = identifierToAdd.Identifier,
                    IdentifierType = identifierType,
                    SourceSystem = sourceSystem
                };

                var griResearchStudyStatus = new GriResearchStudyStatus
                {
                    Code = request.RequestContext.StatusCode,
                    ResearchStudyIdentifier = griMapping,
                    FromDate = DateTime.Now
                };

                await _context.AddAsync(griMapping, cancellationToken);

                await _context.AddAsync(griResearchStudyStatus, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return await GetAsync(request.RequestContext.Identifier);
        }

        public async Task<GovernmentResearchIdentifier> GetAsync(string identifier, CancellationToken cancellationToken = default)
        {
            var griResearchStudy = await GetGriResearchStudyByIdentifierAsync(identifier, cancellationToken) ?? throw new GriNotFoundException();

            var governmentResearchIdentifier = Map(griResearchStudy);

            return governmentResearchIdentifier;
        }

        private GovernmentResearchIdentifier Map(GriResearchStudy griResearchStudy)
        {
            var linkedSystemIdentifiers = new List<LinkedSystemIdentifier>();

            foreach (var x in griResearchStudy.ResearchStudyIdentifiers)
            {
                linkedSystemIdentifiers.Add(new LinkedSystemIdentifier
                {
                    CreatedAt = x.Created,
                    SystemName = x.SourceSystem.Code,
                    Identifier = x.Value,
                    IdentifierType = x.IdentifierType.Description,
                    StatusCode = x.IdentifierStatuses.FirstOrDefault(status => !status.ToDate.HasValue)?.Code ?? ""
                });
            }

            return new GovernmentResearchIdentifier
            {
                Created = griResearchStudy.Created,
                LinkedSystemIdentifiers = linkedSystemIdentifiers,
                Identifier = griResearchStudy.Gri,
                ShortTitle = griResearchStudy.ShortTitle,
                TeamMembers = Map(griResearchStudy.ResearchStudyTeamMembers)
            };
        }

        private List<TeamMember> Map(ICollection<ResearchStudyTeamMember> researchStudyTeamMembers)
        {
            var teamMembers = new List<TeamMember>();

            foreach (var teamMember in researchStudyTeamMembers)
            {
                teamMembers.Add(new TeamMember
                {
                    Role = new Role
                    {
                        Description = teamMember.PersonRole != null
                            && !string.IsNullOrEmpty(teamMember.PersonRole.Description)
                            ? teamMember.PersonRole.Description : "",
                        Name = teamMember.PersonRole != null
                            && !string.IsNullOrEmpty( teamMember.PersonRole.Code)
                            ? teamMember.PersonRole.Code
                            : ""
                    },
                    Person = new PersonWithPrimaryEmail
                    {
                        Email = new Email
                        {
                            Address = teamMember.Researcher.Person.PersonNames.First().Email
                        },
                        Firstname = teamMember.Researcher.Person.PersonNames.First().Given,
                        Lastname = teamMember.Researcher.Person.PersonNames.First().Family
                    }
                });
            }

            return teamMembers;
        }

        private async Task<GriResearchStudy?> GetGriResearchStudyByIdentifierAsync(string? identifier,
            CancellationToken cancellationToken)
        {
            var griResearchStudy = await _context.GriResearchStudies
                .Include(context => context.ResearchStudyTeamMembers)
                    .ThenInclude(x => x.Researcher)
                    .ThenInclude(researcher => researcher.Person)
                    .ThenInclude(person => person.PersonNames)
                 .Include(context => context.ResearchStudyTeamMembers).ThenInclude(x => x.PersonRole)
                 .Include(study => study.ResearchStudyIdentifiers).ThenInclude(mapping => mapping.SourceSystem)
                 .Include(study => study.ResearchStudyIdentifiers).ThenInclude(x => x.IdentifierType)
                 .Include(study => study.ResearchStudyIdentifiers).ThenInclude(mapping => mapping.IdentifierStatuses)
                .FirstOrDefaultAsync(x => x.Gri == identifier, cancellationToken);

            return griResearchStudy;
        }

        private async Task<PersonDb?> GetPersonAsync(PersonWithPrimaryEmail person,
            CancellationToken cancellationToken)
        {
            if (person == null)
            {
                return null;
            }

            var personFromDb = await _context.PersonNames.Include(personName => personName.Person)
                .FirstOrDefaultAsync(personName => personName.Given == person.Firstname
                && personName.Family == person.Lastname
                && personName.Email == person.Email.Address, cancellationToken);

            return personFromDb?.Person;
        }


        private async Task<SourceSystem?> GetSourceSystemAsync(string code, CancellationToken cancellationToken)
        {
            return await _context.SourceSystems.FirstOrDefaultAsync(system => system.Code == code, cancellationToken);
        }

    }
}
