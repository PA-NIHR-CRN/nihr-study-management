
namespace NIHR.StudyManagement.Domain.Configuration
{
    public class MessageBusSettings
    {
        public string BootstrapServers { get; set; }

        public string Topic { get; set; }

        public MessageBusSettings()
        {
            BootstrapServers = "";
            Topic = "";
        }
    }
}
