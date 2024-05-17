using Hl7.Fhir.Model;
using NIHR.StudyManagement.Api.Models;
using NIHR.StudyManagement.Domain.Models;

namespace NIHR.StudyManagement.Api.Mappers
{
    public interface IFhirMapper
    {
        RegisterStudyRequest MapCreateRequestBundle(Bundle bundle, string apiSystemName, string identifier);

        Bundle MapToResearchStudyBundle(GovernmentResearchIdentifier governmentResearchIdentifier, HttpRequestResponseFhirContext httpRequestResponseFhirContext);
    }
}
