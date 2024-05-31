using NIHR.StudyManagement.Domain.Models;

namespace NIHR.StudyManagement.Domain.Abstractions
{
    public interface INsipGrisMessageHelper
    {
        NsipMessage<string> Prepare(string eventType, string sourceSystem, string eventData);
    }
}
