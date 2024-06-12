namespace NIHR.StudyManagement.Domain.Models
{
    public class HttpRequestResponseFhirContext
    {
        public HttpMethod Method { get; set; }

        public string Url { get; set; }

        public int Status { get; set; }

        public HttpRequestResponseFhirContext()
        {
            Url = "";
            Method = HttpMethod.Get;
        }
    }
}
