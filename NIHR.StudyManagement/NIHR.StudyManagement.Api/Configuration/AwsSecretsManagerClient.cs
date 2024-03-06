using System.Text.Json;
using Amazon.SecretsManager;
using Amazon;
using Amazon.SecretsManager.Model;

namespace NIHR.StudyManagement.Api.Configuration;

public class AwsSecretsManagerClient
{
    private readonly RegionEndpoint _region;
    private readonly string _secretName;

    public AwsSecretsManagerClient(RegionEndpoint region, string secretName)
    {
        _region = region;
        _secretName = secretName;
    }

    public Dictionary<string, string> Load()
    {
        var secret = GetSecret();

        var data = JsonSerializer.Deserialize<Dictionary<string, string>>(secret);

        return data;
    }

    private string GetSecret()
    {
        var request = new GetSecretValueRequest
        {
            SecretId = _secretName,
            VersionStage = "AWSCURRENT" // VersionStage defaults to AWSCURRENT if unspecified.
        };

        using (var client =
        new AmazonSecretsManagerClient(_region))
        {
            var response = client.GetSecretValueAsync(request).Result;

            string secretString;
            if (response.SecretString != null)
            {
                secretString = response.SecretString;
            }
            else
            {
                var memoryStream = response.SecretBinary;
                var reader = new StreamReader(memoryStream);
                secretString = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));
            }

            return secretString;
        }
    }
}

