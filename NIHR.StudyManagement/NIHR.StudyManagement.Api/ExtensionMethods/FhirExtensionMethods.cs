using Hl7.Fhir.Model;
using NIHR.StudyManagement.Api.Models;

namespace NIHR.StudyManagement.Api.ExtensionMethods
{
    public static class FhirExtensionMethods
    {
        public static void AddNewEntryComponent(this Bundle bundle,
            Resource resource,
            HttpRequestResponseFhirContext httpRequestResponseFhirContext)
        {
            var bundleRequestContext = new Bundle.RequestComponent
            {
                Method = MapToBundleHttpVerb(httpRequestResponseFhirContext.Method),
                Url = httpRequestResponseFhirContext.Url
            };

            var bundleResponseContext = new Bundle.ResponseComponent
            {
                Status = httpRequestResponseFhirContext.Status.ToString()
            };

            if(bundle.Entry == null)
            {
                bundle.Entry = new List<Bundle.EntryComponent>();
            }

            bundle.Entry.Add(new Bundle.EntryComponent()
            {
                Resource = resource,
                Response = bundleResponseContext,
                Request = bundleRequestContext
            });
        }

        private static Bundle.HTTPVerb MapToBundleHttpVerb(HttpMethod httpMethod)
        {
            return httpMethod == HttpMethod.Get
                ? Bundle.HTTPVerb.GET
                : httpMethod == HttpMethod.Delete
                ? Bundle.HTTPVerb.DELETE
                : httpMethod == HttpMethod.Post
                ? Bundle.HTTPVerb.POST
                : httpMethod == HttpMethod.Put
                ? Bundle.HTTPVerb.PUT
                : httpMethod == HttpMethod.Patch
                ? Bundle.HTTPVerb.PATCH
                : Bundle.HTTPVerb.HEAD;
        }
    }
}
