using NIHR.StudyManagement.Api.Controllers;
using NIHR.StudyManagement.Api.Models;
using NIHR.StudyManagement.Api.Models.Dto;
using NIHR.StudyManagement.Domain.Models;

namespace NIHR.StudyManagement.Api.Mappers
{
    public class GovernmentResearchIdentifierDtoMapper : IGovernmentResearchIdentifierDtoMapper
    {
        public RegisterStudyToExistingIdentifierRequest Map(RegisterStudyRequestDto requestDto, string identifier)
        {
            var chiefInvestigatorPerson = requestDto.TeamMembers.First();

            var createIdentifierRequest = new RegisterStudyToExistingIdentifierRequest()
            {
                ShortTitle = requestDto.LocalStudy.ShortTitle,
                ProjectId = requestDto.LocalStudy.ProjectId,
                ProtocolId = requestDto.ProtocolId,
                Identifier = identifier,
                ChiefInvestigator = new PersonWithPrimaryEmail
                {
                    Email = new Email { Address = chiefInvestigatorPerson.PrimaryEmail },
                    Firstname = chiefInvestigatorPerson.Firstname,
                    Lastname = chiefInvestigatorPerson.Lastname
                }
            };

            return createIdentifierRequest;
        }

        public RegisterStudyRequest Map(RegisterStudyRequestDto requestDto)
        {
            var chiefInvestigatorPerson = requestDto.TeamMembers.First();

            var createIdentifierRequest = new RegisterStudyRequest()
            {
                ShortTitle = requestDto.LocalStudy.ShortTitle,
                ProjectId = requestDto.LocalStudy.ProjectId,
                ProtocolId = requestDto.ProtocolId,
                ChiefInvestigator = new PersonWithPrimaryEmail
                {
                    Email = new Email { Address = chiefInvestigatorPerson.PrimaryEmail},
                    Firstname = chiefInvestigatorPerson.Firstname,
                    Lastname = chiefInvestigatorPerson.Lastname
                }
            };

            return createIdentifierRequest;
        }

        public GovernmentResearchIdentifierDto Map(GovernmentResearchIdentifier governmentResearchIdentifier)
        {
            var identifier = new GovernmentResearchIdentifierDto
                {
                    CreatedAt = governmentResearchIdentifier.Created,
                    Gri = governmentResearchIdentifier.Identifier,
                    ShortTitle = governmentResearchIdentifier.ShortTitle,
                    TeamMembers = Map(governmentResearchIdentifier.TeamMembers),
                    LinkedSystemIdentifiers = Map(governmentResearchIdentifier.LinkedSystemIdentifiers)
                };

            return identifier;
        }

        private static List<LinkedSystemIdentifierDto> Map(List<LinkedSystemIdentifier> linkedSystemIdentifiers)
        {
            var linkedIdentifierDtos = new List<LinkedSystemIdentifierDto>();

            foreach (var linkedSystemIdentifier in linkedSystemIdentifiers)
            {
                linkedIdentifierDtos.Add(new LinkedSystemIdentifierDto
                {
                    Identifier = linkedSystemIdentifier.Identifier,
                    SystemName = linkedSystemIdentifier.SystemName,
                    CreatedAt = linkedSystemIdentifier.CreatedAt
                });
            }

            return linkedIdentifierDtos;
        }

        private List<TeamMemberDto> Map(List<TeamMember> teamMembers)
        {
            var teamMemberDtos = new List<TeamMemberDto>();

            foreach (var teamMember in teamMembers)
            {
                teamMemberDtos.Add(new TeamMemberDto {
                    Role = new RoleDto {
                        Name = teamMember.Role.Description
                    },
                    EffectiveFrom = teamMember.EffectiveFrom,
                    EffectiveTo = teamMember.EffectiveTo,
                    Person = new PersonWithEmailDto {
                        Firstname = teamMember.Person.Firstname,
                        Lastname = teamMember.Person.Lastname,
                        Email = teamMember.Person.Email.Address
                    }
                });
            }

            return teamMemberDtos;
        }
    }
}
