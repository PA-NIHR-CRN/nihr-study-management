using Hl7.Fhir.Model;
using NIHR.StudyManagement.Domain.Models;

namespace NIHR.StudyManagement.Domain.Abstractions
{
    public interface IFhirMapper
    {
        RegisterStudyRequest MapCreateRequestBundle(Bundle bundle, string apiSystemName, string identifier);

        Bundle MapToResearchStudyBundle(GovernmentResearchIdentifier governmentResearchIdentifier, HttpRequestResponseFhirContext httpRequestResponseFhirContext);

        string MapToResearchStudyBundleAsJson(GovernmentResearchIdentifier governmentResearchIdentifier, HttpRequestResponseFhirContext httpRequestResponseFhirContext);
    }
}
