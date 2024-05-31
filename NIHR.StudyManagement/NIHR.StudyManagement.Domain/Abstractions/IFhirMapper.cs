using Hl7.Fhir.Model;
using NIHR.StudyManagement.Domain.Models;

namespace NIHR.StudyManagement.Domain.Abstractions
{
    public interface IFhirMapper
    {
        RegisterStudyRequest MapCreateRequestBundle(Bundle bundle);

        Bundle MapToResearchStudyBundle(GovernmentResearchIdentifier governmentResearchIdentifier);

        string MapToResearchStudyBundleAsJson(GovernmentResearchIdentifier governmentResearchIdentifier);
    }
}
