using Hl7.Fhir.Model;
using Hl7.Fhir.Support;
using NIHR.StudyManagement.Api.ExtensionMethods;
using NIHR.StudyManagement.Api.Models;
using NIHR.StudyManagement.Domain.EnumsAndConstants;
using NIHR.StudyManagement.Domain.Models;
using NIHR.StudyManagement.Infrastructure.Repository.EnumsAndConstants;
using static Hl7.Fhir.ElementModel.ScopedNode;

namespace NIHR.StudyManagement.Api.Mappers
{

    public class FhirMapper : IFhirMapper
    {
        public RegisterStudyRequest MapCreateRequestBundle(Bundle bundle,
            string apiSystemName,
            string identifier)
        {
            if(bundle == null)
            {
                throw new ArgumentNullException(nameof(bundle));
            }

            var bundleResources = bundle.GetResources();

            var researchStudyResource = bundleResources.FirstOrDefault(resource => resource.TypeName.Equals("ResearchStudy", StringComparison.OrdinalIgnoreCase));

            var researchStudy = researchStudyResource as ResearchStudy;

            if (researchStudy == null) { throw new ArgumentException("Could not map bundle."); }

            var identifiers = GetIdentifiers(bundle, researchStudy);
            var practitioners = GetPractitioners(bundleResources, researchStudy);

            var grisIdentifier = "";

            foreach (var linkedIdentifier in identifiers)
            {
                if(linkedIdentifier.Type == ResearchInitiativeIdentifierTypes.GrisId)
                {
                    grisIdentifier = linkedIdentifier.Value;

                    if(grisIdentifier != identifier)
                    {
                        throw new ArgumentException($"Identifier '{grisIdentifier}' does not match the identifier '{identifier}' specified in the route.");
                    }
                    break;
                }
            }

            var registerStudyRequest = string.IsNullOrEmpty(grisIdentifier)
                ? new RegisterStudyRequest()
                : new RegisterStudyToExistingIdentifierRequest() { Identifier = grisIdentifier };

            registerStudyRequest.ShortTitle = researchStudy.Label?.First()?.Value ?? "";
            registerStudyRequest.ApiSystemName = apiSystemName;
            registerStudyRequest.TeamMembers = practitioners;
            registerStudyRequest.Identifiers = identifiers;

            return registerStudyRequest;
        }

        private List<TeamMember> GetPractitioners(IEnumerable<Resource> bundleResources, ResearchStudy researchStudy)
        {
            var teamMembers = new List<TeamMember>();

            // Iterate over each associated party in study
            foreach (var associatedParty in researchStudy.AssociatedParty)
            {
                PractitionerRole practitionerRole = null;

                // Find the matching resource
                foreach (var resource in bundleResources)
                {
                    if (resource.Id.Equals(associatedParty.Party.Reference, StringComparison.OrdinalIgnoreCase)
                        && resource is PractitionerRole)
                    {
                        practitionerRole = (PractitionerRole)resource;
                        break;
                    }
                }

                if(practitionerRole == null)
                {
                    continue;
                }

                foreach (var resource in bundleResources)
                {
                    if (resource.Id.Equals(practitionerRole.Practitioner.Reference.Trim('#'), StringComparison.OrdinalIgnoreCase)
                                            && resource is Practitioner)
                    {
                        var practitioner = (Practitioner)resource;

                        var name = practitioner.Name.FirstOrDefault(x => x.Use == HumanName.NameUse.Usual);
                        var telecom = practitioner.Telecom.FirstOrDefault(x => x.System == ContactPoint.ContactPointSystem.Email && x.Use == ContactPoint.ContactPointUse.Work);

                        var teamMember = new TeamMember {
                            Role = new Role
                            {
                                Description = practitionerRole.Code.First().Coding.First().Display,
                                Name = practitionerRole.Code.First().Coding.First().Display,
                            }
                        };

                        if (name != null)
                        {
                            teamMember.Person.Firstname = string.Join(" ", name.Given);
                            teamMember.Person.Lastname = name.Family;
                        }

                        if (telecom != null)
                        {
                            teamMember.Person.Email = new Email
                            {
                                Address = telecom.Value
                            };
                        }

                        teamMembers.Add(teamMember);
                    }
                }
            }
            return teamMembers;
        }

        public Bundle MapToResearchStudyBundle(GovernmentResearchIdentifier x, HttpRequestResponseFhirContext httpRequestResponseFhirContext)
        {
            var bundle = new Bundle
            {
                Id = Guid.NewGuid().ToString(),
                Type = Bundle.BundleType.Searchset,
                Timestamp = DateTime.Now
            };

            bundle.Entry = new List<Bundle.EntryComponent>();

            // Initialise and set short title
            var study = new ResearchStudy()
            {
                Id = x.Identifier,
                Label = new List<ResearchStudy.LabelComponent> {
                    new ResearchStudy.LabelComponent
                    {
                         Value = x.ShortTitle,
                         Type = new CodeableConcept{
                             Text = FhirResourceAttributeDescriptions.LabelShortTitle
                         }
                    }
                },
                Status = PublicationStatus.Active
            };
            study.Identifier = new List<Identifier>();

            study.Identifier.AddRange(GetLinkedIdentifiers(x));

            // Add the GRIS ID as an identifier in the response
            study.Identifier.Add(new Identifier
            {
                Use = Identifier.IdentifierUse.Official,
                Type = new CodeableConcept()
                {
                    Text = ResearchInitiativeIdentifierTypes.GrisId
                },
                Value = x.Identifier,
                Period = new Period {
                    Start = x.Created.ToFhirDate()
                }
            });

            var firstPractitioner = AddTeamMembers(x, bundle, httpRequestResponseFhirContext);

            var practitionerRole = GetPractitionerRole(x, firstPractitioner?.Id ?? "");

            bundle.AddNewEntryComponent(practitionerRole, httpRequestResponseFhirContext);

            study.AssociatedParty = new List<ResearchStudy.AssociatedPartyComponent> {
                new ResearchStudy.AssociatedPartyComponent{
                    Party = new ResourceReference() { Reference = $"#{practitionerRole.Id}"},
                    Role = new CodeableConcept
                    {
                        Coding = new List<Coding>{

                            new Coding{
                                Display = PersonRoles.ChiefInvestigator
                            }
                        }
                    }
                }
            };

            bundle.AddNewEntryComponent(study, httpRequestResponseFhirContext);

            return bundle;
        }

        private List<ResearchInitiativeIdentifierItem> GetIdentifiers(Bundle bundle, ResearchStudy researchStudy)
        {
            var identifiers = new List<ResearchInitiativeIdentifierItem>();

            // Add each identifier from bundle.
            foreach (var identifier in researchStudy.Identifier)
            {
                identifiers.Add(new ResearchInitiativeIdentifierItem
                {
                    Type = identifier.Type.Text,
                    Value = identifier.Value,
                    StatusCode = researchStudy.Status.HasValue
                ? researchStudy.Status.Value.ToString()
                : ""
                });
            }

            // Capture bundle
            identifiers.Add(new ResearchInitiativeIdentifierItem {
                Type = ResearchInitiativeIdentifierTypes.Bundle,
                Value = bundle.Id,
                Created = bundle.Timestamp.HasValue ? bundle.Timestamp.Value.UtcDateTime : DateTime.Now
            });

            return identifiers;
        }

        private static Practitioner? AddTeamMembers(GovernmentResearchIdentifier x, Bundle bundle,
            HttpRequestResponseFhirContext httpRequestResponseFhirContext)
        {
            Practitioner firstPractitioner = null;

            foreach (var teamMember in x.TeamMembers)
            {
                var practitioner = new Practitioner()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = new List<HumanName>
                {
                    new HumanName
                    {
                        Use = HumanName.NameUse.Usual,
                        Family = teamMember.Person.Lastname,
                        Given = new List<string> { teamMember.Person.Firstname }
                    }
                },
                    Telecom = new List<ContactPoint>
                    {
                        new ContactPoint
                        {
                            Use = ContactPoint.ContactPointUse.Work,
                            System = ContactPoint.ContactPointSystem.Email,
                            Value = teamMember.Person.Email.Address
                        }
                    }
                };

                bundle.AddNewEntryComponent(practitioner, httpRequestResponseFhirContext);

                firstPractitioner = practitioner;
            }

            return firstPractitioner;
        }

        private static PractitionerRole GetPractitionerRole(GovernmentResearchIdentifier x,
            string practitionerId)
        {
            return new PractitionerRole()
            {
                Id = Guid.NewGuid().ToString(),
                Practitioner = new ResourceReference() { Reference = $"#{practitionerId}" },
                Code = new List<CodeableConcept> {
                    new CodeableConcept{
                        Coding = new List<Coding>{

                            new Coding{
                                Display = PersonRoles.ChiefInvestigator
                            }
                        }
                    }
                }
            };
        }

        private static IEnumerable<Identifier> GetLinkedIdentifiers(GovernmentResearchIdentifier x)
        {
            foreach (var y in x.LinkedSystemIdentifiers)
            {
                if(y.IdentifierType == ResearchInitiativeIdentifierTypes.Bundle
                    || y.IdentifierType == ResearchInitiativeIdentifierTypes.GrisId)
                {
                    continue;
                }

                yield return new Identifier
                {
                    Use = Identifier.IdentifierUse.Usual,
                    Type = new CodeableConcept()
                    {
                        Text = y.IdentifierType
                    },
                    Value = y.Identifier,
                    System = y.SystemName,
                    Period = new Period {
                        Start = y.CreatedAt.ToFhirDate(),
                    }
                };
            }
        }

        private string GetProtocolId(ResearchStudy researchStudy)
        {
            return GetIdentifierByTypeText("protocol id", researchStudy);
        }

        private static string GetProjectId(ResearchStudy researchStudy)
        {
            return GetIdentifierByTypeText("edge id", researchStudy);
        }

        private static string GetIdentifierByTypeText(string typeText, ResearchStudy researchStudy)
        {
            var identifierValue = "";

            foreach (var identifier in researchStudy.Identifier)
            {
                if (identifier.Type.Text.Equals(typeText, StringComparison.OrdinalIgnoreCase))
                {
                    identifierValue = identifier.Value;
                    break;
                }
            }

            return identifierValue;
        }

        private static PersonWithPrimaryEmail GetChiefInvestigator(Bundle bundle)
        {
            var chief = new PersonWithPrimaryEmail();
            var bundleResources = bundle.GetResources();
            var practitionerId = "";


            foreach (var resource in bundleResources)
            {
                if (resource is not PractitionerRole)
                {
                    continue;
                }

                var practitionerRole = (PractitionerRole) resource;

                practitionerId = practitionerRole.Practitioner.Reference;

                break;
            }

            foreach (var resource in bundleResources)
            {
                if (resource is not Practitioner
                    || resource.Id != practitionerId.Trim('#'))
                {
                    continue;
                }

                var practitioner = (Practitioner) resource;

                var name = practitioner.Name.FirstOrDefault(x => x.Use == HumanName.NameUse.Usual);
                var telecom = practitioner.Telecom.FirstOrDefault(x => x.System == ContactPoint.ContactPointSystem.Email && x.Use == ContactPoint.ContactPointUse.Work);

                if (name != null)
                {
                    chief.Firstname = string.Join(" ", name.Given);
                    chief.Lastname = name.Family;
                }

                if (telecom != null)
                {
                    chief.Email = new Email
                    {
                        Address = telecom.Value
                    };
                }

                break;
            }

            return chief;
        }
    }
}
