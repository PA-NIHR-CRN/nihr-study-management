using Hl7.Fhir.Model;
using NIHR.StudyManagement.Domain.Models;

namespace NIHR.StudyManagement.Api.Mappers
{
    public class FhirMapper : IFhirMapper
    {
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
                ChiefInvestigator = GetChiefInvestigator(bundle)
            };

            return registerStudyRequest;
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

        private static string GetIdentifierByCodeDisplay(string codeDisplay, ResearchStudy researchStudy)
        {
            var identifierValue = "";

            foreach (var identifier in researchStudy.Identifier)
            {
                foreach (var coding in identifier.Type.Coding)
                {
                    if (coding.Display.Equals(codeDisplay, StringComparison.OrdinalIgnoreCase))
                    {
                        identifierValue = identifier.Value;
                        break;
                    }
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
                    || resource.Id != practitionerId)
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
