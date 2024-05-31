using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using NIHR.StudyManagement.Domain.Abstractions;
using NIHR.StudyManagement.Domain.Models;
using NIHR.StudyManagement.Infrastructure.Repository.EnumsAndConstants;
using System.Text.Json;

namespace NIHR.StudyManagement.Api.Mappers
{
    public class FhirMapper : IFhirMapper
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public FhirMapper(JsonSerializerOptions jsonSerializerOptions)
        {
            this._jsonSerializerOptions = jsonSerializerOptions;
        }

        public string MapToResearchStudyBundleAsJson(GovernmentResearchIdentifier governmentResearchIdentifier)
        {
            // Map to bundle object
            var bundle = MapToResearchStudyBundle(governmentResearchIdentifier);

            // Serialize the bundle using FHIR serialization options.
            var bundleJson = JsonSerializer.Serialize(bundle, _jsonSerializerOptions);

            return bundleJson;
        }

        public RegisterStudyRequest MapCreateRequestBundle(Bundle bundle)
        {
            if(bundle == null)
            {
                throw new ArgumentNullException(nameof(bundle));
            }

            var bundleResources = bundle.GetResources();

            var researchStudyResource = bundleResources.FirstOrDefault(resource => resource.TypeName.Equals("ResearchStudy", StringComparison.OrdinalIgnoreCase));

            var researchStudy = researchStudyResource as ResearchStudy;

            if (researchStudy == null) { throw new ArgumentException("Could not map bundle."); }

            var registerStudyRequest = new RegisterStudyRequest
            {
                ShortTitle = researchStudy.Label?.First()?.Value ?? "",
                ProjectId = GetProjectId(researchStudy),
                ProtocolId = GetProtocolId(researchStudy),
                StatusCode = researchStudy.Status.HasValue
                            ? researchStudy.Status.Value.ToString()
                            : "",
                Identifiers = GetIdentifiers(bundle, researchStudy),
                ChiefInvestigator = GetChiefInvestigator(bundle)
            };

            return registerStudyRequest;
        }

        public Bundle MapToResearchStudyBundle(GovernmentResearchIdentifier x)
        {
            var bundle = new Bundle
            {
                Type = Bundle.BundleType.Document
            };

            bundle.Entry = new List<Bundle.EntryComponent>();

            // Initialise and set short title
            var study = new ResearchStudy()
            {
                Id = x.Identifier,
                Label = new List<ResearchStudy.LabelComponent> {
                    new ResearchStudy.LabelComponent
                    {
                         Value = x.ShortTitle
                    }
                },
                Status = PublicationStatus.Active
            };
            study.Identifier = new List<Identifier>();

            study.Identifier.AddRange(GetLinkedIdentifiers(x));

            var firstPractitioner = AddTeamMembers(x, bundle);

            var practitionerRole = GetPractitionerRole(x, firstPractitioner?.Id ?? "");

            bundle.Entry.Add(new Bundle.EntryComponent() { Resource = practitionerRole });

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

            bundle.Entry.Add(new Bundle.EntryComponent() { Resource = study });

            return bundle;
        }

        private List<ResearchInitiativeIdentifierItem> GetIdentifiers(Bundle bundle, ResearchStudy researchStudy)
        {
            var identifiers = new List<ResearchInitiativeIdentifierItem>();

            // Add each identifier from bundle.
            foreach (var identifier in researchStudy.Identifier)
            {
                identifiers.Add(new ResearchInitiativeIdentifierItem { Type = identifier.Type.Text, Value = identifier.Value });
            }

            // Capture bundle
            identifiers.Add(new ResearchInitiativeIdentifierItem {
                Type = ResearchInitiativeIdentifierTypes.Bundle,
                Value = bundle.Id,
                Created = bundle.Timestamp.HasValue ? bundle.Timestamp.Value.UtcDateTime : DateTime.Now
            });

            return identifiers;
        }

        private static Practitioner? AddTeamMembers(GovernmentResearchIdentifier x, Bundle bundle)
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

                bundle.Entry.Add(new Bundle.EntryComponent() { Resource = practitioner });

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
                    Value = y.Identifier
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
