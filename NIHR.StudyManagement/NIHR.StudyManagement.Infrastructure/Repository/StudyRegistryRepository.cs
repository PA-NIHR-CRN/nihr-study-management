using Microsoft.EntityFrameworkCore;
using NIHR.StudyManagement.Domain.Abstractions;
using NIHR.StudyManagement.Domain.EnumsAndConstants;
using NIHR.StudyManagement.Domain.Exceptions;
using NIHR.StudyManagement.Domain.Models;
using NIHR.StudyManagement.Infrastructure.Repository.Models;

using PersonDb = NIHR.StudyManagement.Infrastructure.Repository.Models.PersonEntity;

namespace NIHR.StudyManagement.Infrastructure.Repository
{
    public class StudyRegistryRepository : IStudyRegistryRepository
    {
        private readonly StudyRegistryContext _context;

        public StudyRegistryRepository(StudyRegistryContext context)
        {
            _context = context;
        }

        public async Task<GovernmentResearchIdentifier> CreateAsync(RegisterStudyRequest request,
            string griId,
            CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            };

            var localSystem = await GetSourceSystemAsync(request.ApiSystemName, cancellationToken) ?? throw new EntityNotFoundException(nameof(SourceSystemEntity));

            var researchInitiativeIdentifierTypes = new Dictionary<string, ResearchStudyIdentifierTypeEntity>();

            foreach (var x in _context.ResearchInitiativeIdentifierTypes.Select(x => x))
            {
                researchInitiativeIdentifierTypes[x.Description] = x;
            }

            var researchStudy = new ResearchStudyEntity
            {
                ShortTitle = request.ShortTitle,
                RequestSourceSystem = localSystem
            };

            foreach (var identifier in request.Identifiers)
            {

                var griMapping = new ResearchStudyIdentifierEntity
                {
                    ResearchStudy = researchStudy,
                    Value = identifier.Value,
                    IdentifierType = researchInitiativeIdentifierTypes[identifier.Type],
                    SourceSystem = localSystem
                };

                var identifierStatus = new ResearchStudyIdentifierStatusEntity
                {
                    ResearchStudyIdentifier = griMapping,
                    Code = identifier.StatusCode
                };

                await _context.AddAsync(identifierStatus);
                await _context.AddAsync(griMapping, cancellationToken);
            }

            foreach (var teamMember in request.TeamMembers)
            {
                OrganisationEntity? organisation = null;

                if (teamMember.Organisation != null)
                {
                    organisation = await _context.OrganisationEntities
                        .FirstOrDefaultAsync(org => org.Code == teamMember.Organisation.Code)
                        ?? new OrganisationEntity { Code = teamMember.Organisation.Code, Description = teamMember.Organisation.Description };
                }

                var personEntity = await GetPersonAsync(teamMember.Person, cancellationToken) ?? new PersonDb
                {
                    PersonNames = new PersonNameEntity[] {
                        new PersonNameEntity {
                        Given =teamMember.Person.Firstname,
                        Family = teamMember.Person.Lastname,
                        Email = teamMember.Person.Email.Address
                        }
                    }
                };

                var roleType = await _context.PersonRoles.FirstOrDefaultAsync(x => x.Code == teamMember.Role.Code, cancellationToken) ?? throw new EntityNotFoundException(nameof(RoleTypeEntity));

                var teamMemberToAdd = new ResearchStudyTeamMemberEntity
                {
                    ResearchStudy = researchStudy,
                    Practitioner = new PractitionerEntity
                    {
                        Person = personEntity
                    },
                    PersonRole = roleType,
                    Organitation = organisation
                };

                await _context.AddAsync(teamMemberToAdd, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return await GetAsync(griId);
        }

        public async Task<GovernmentResearchIdentifier> AddStudyToIdentifierAsync(AddStudyToExistingIdentifierRequestWithContext request,
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var griResearchStudy = await GetGriResearchStudyByIdentifierAsync(request.RequestContext.Identifier, cancellationToken) ?? throw new GriNotFoundException();

            var sourceSystem = await GetSourceSystemAsync(request.RequestContext.ApiSystemName, cancellationToken) ?? throw new EntityNotFoundException(nameof(SourceSystemEntity));

            var researchInitiativeIdentifierTypes = new Dictionary<string, ResearchStudyIdentifierTypeEntity>();

            foreach (var x in _context.ResearchInitiativeIdentifierTypes.Select(x => x))
            {
                researchInitiativeIdentifierTypes[x.Description] = x;
            }

            foreach (var identifierToAdd in request.LinkedSystemIdentifiersToAdd)
            {
                var identifierType = researchInitiativeIdentifierTypes[identifierToAdd.IdentifierType];

                var griMapping = new ResearchStudyIdentifierEntity
                {
                    ResearchStudy = griResearchStudy,
                    Value = identifierToAdd.Identifier,
                    IdentifierType = identifierType,
                    SourceSystem = sourceSystem
                };

                var griResearchStudyStatus = new ResearchStudyIdentifierStatusEntity
                {
                    Code = identifierToAdd.StatusCode,
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

        private GovernmentResearchIdentifier Map(ResearchStudyEntity griResearchStudy)
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
                ShortTitle = griResearchStudy.ShortTitle,
                TeamMembers = Map(griResearchStudy.ResearchStudyTeamMembers)
            };
        }

        private List<TeamMember> Map(ICollection<ResearchStudyTeamMemberEntity> researchStudyTeamMembers)
        {
            var teamMembers = new List<TeamMember>();

            foreach (var teamMemberEntity in researchStudyTeamMembers)
            {
                var teamMember = new TeamMember
                {
                    Role = new Role
                    {
                        Description = teamMemberEntity.PersonRole != null
                            && !string.IsNullOrEmpty(teamMemberEntity.PersonRole.Description)
                            ? teamMemberEntity.PersonRole.Description : "",
                        Code = teamMemberEntity.PersonRole != null
                            && !string.IsNullOrEmpty(teamMemberEntity.PersonRole.Code)
                            ? teamMemberEntity.PersonRole.Code
                            : ""
                    },
                    Person = new PersonWithPrimaryEmail
                    {
                        Email = new Email
                        {
                            Address = teamMemberEntity.Practitioner.Person.PersonNames.First().Email
                        },
                        Firstname = teamMemberEntity.Practitioner.Person.PersonNames.First().Given,
                        Lastname = teamMemberEntity.Practitioner.Person.PersonNames.First().Family
                    }
                };

                if(teamMemberEntity.Organitation != null)
                {
                    teamMember.Organisation = new Organisation(teamMemberEntity.Organitation.Code,
                        teamMemberEntity.Organitation.Description);
                }

                teamMembers.Add(teamMember);
            }

            return teamMembers;
        }

        private async Task<ResearchStudyEntity?> GetGriResearchStudyByIdentifierAsync(string? identifier,
            CancellationToken cancellationToken)
        {
            var griResearchStudyIdentifier = await _context.ResearchStudyIdentifiers
                .Include(record => record.SourceSystem)
                .Include(record => record.IdentifierStatuses)
                .Include(record => record.IdentifierType)
                .Include(record => record.ResearchStudy)
                    .ThenInclude(study => study.ResearchStudyTeamMembers)
                    .ThenInclude(team => team.Practitioner)
                    .ThenInclude(team => team.Person)
                    .ThenInclude(team => team.PersonNames)
                .FirstOrDefaultAsync(researchStudyIdentifier => researchStudyIdentifier.IdentifierType.Description == ResearchInitiativeIdentifierTypes.GrisId
                    && researchStudyIdentifier.Value == identifier, cancellationToken);

            return griResearchStudyIdentifier?.ResearchStudy;
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


        private async Task<SourceSystemEntity?> GetSourceSystemAsync(string code, CancellationToken cancellationToken)
        {
            return await _context.SourceSystems.FirstOrDefaultAsync(system => system.Code == code, cancellationToken);
        }

        public async Task<bool> DoesGrisExist(string identifier, CancellationToken cancellationToken = default)
        {
            var matchingIdentifiers = from existingIdentifiers in _context.ResearchStudyIdentifiers
                                      join identifierType in _context.ResearchInitiativeIdentifierTypes on existingIdentifiers.IdentifierTypeId equals identifierType.Id
                                      where identifierType.Description == ResearchInitiativeIdentifierTypes.GrisId && existingIdentifiers.Value == identifier
                                      select existingIdentifiers;

            return await matchingIdentifiers.FirstOrDefaultAsync() != null;
        }
    }
}
