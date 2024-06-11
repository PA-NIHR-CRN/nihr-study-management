
using Amazon;
using NIHR.StudyManagement.Infrastructure.AWS;

namespace NIHR.StudyManagement.Infrastructure
{
    public class SharedApplicationnStartup
    {
        public static string GetAwsSecretPassword(string secretName)
        {
            var secretManager = new AwsSecretsManagerClient(RegionEndpoint.EUWest2, secretName);

            var data = secretManager.Load();

            if (data.ContainsKey("password"))
            {
                return data["password"];
            }

            return string.Empty;
        }
    }
}
