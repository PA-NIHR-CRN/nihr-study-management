namespace NIHR.StudyManagement.Domain.Models
{
    /// <summary>
    /// Message wrapper for an NSIP message
    /// </summary>
    /// <typeparam name="TEventData"></typeparam>
    public class NsipMessage<TEventData>
    {
        public decimal NsipEventVersion { get; set; }

        public string NsipEventId { get; set; }

        public string NsipEventSourceSystemId { get; set; }

        public string NsipEventTargetSystemId { get; set; }

        public string NsipEventType { get; set; }

        public DateTime NsipEventDateCreated { get; set; }

        public TEventData NsipEventData { get; private set; }

        public NsipMessage(TEventData nsipEventData)
        {
            NsipEventData = nsipEventData;
            NsipEventDateCreated = DateTime.Now;
            NsipEventVersion = 1;
            NsipEventId = "";
            NsipEventSourceSystemId = "";
            NsipEventTargetSystemId = "";
            NsipEventType = "";

        }
    }
}
