using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;

namespace RDD1191.CreateCognitoUser
{
    public class AwsCognitoUserManager
    {
        private string _userPoolId;

        public AwsCognitoUserManager(string userPoolId)
        {
            _userPoolId = string.IsNullOrEmpty(userPoolId)
                ? throw new ArgumentNullException(nameof(userPoolId))
                : userPoolId;
        }

        public async Task<AdminGetUserResponse?> GetUser(string username)
        {
            var cancellationTokenSource = new CancellationTokenSource();

            cancellationTokenSource.CancelAfter(5000);
            var cognitoClient = GetCognitoClient();

            var getUserRequest = new AdminGetUserRequest {
                Username = username,
                UserPoolId = _userPoolId
            };

            try
            {
                var getUserResponse = await cognitoClient.AdminGetUserAsync(getUserRequest, cancellationTokenSource.Token);

                if (getUserResponse.HttpStatusCode != System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine($"Failed to read user profile");
                    return null;
                }

                return getUserResponse;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private AmazonCognitoIdentityProviderClient GetCognitoClient()
        {
            // No credentials passed in here. This assumes credentials of local Visual Studio user .\aws\credentials file.
            var chain = new CredentialProfileStoreChain();
            var result = chain.TryGetAWSCredentials("Default", out var credentials);

            AmazonCognitoIdentityProviderClient cognitoClient = new AmazonCognitoIdentityProviderClient(credentials, RegionEndpoint.EUWest2);

            return cognitoClient;
        }

        public async Task<AdminCreateUserResponse?> CreateUser(string username)
        {
            var cancellationTokenSource = new CancellationTokenSource();

            cancellationTokenSource.CancelAfter(5000);
            var cognitoClient = GetCognitoClient();

            var createUserRequest = new AdminCreateUserRequest
            {
                Username = username,
                UserPoolId = _userPoolId,
                MessageAction = MessageActionType.SUPPRESS,
                UserAttributes = new List<AttributeType>
                {
                    new AttributeType
                    {
                        Name = "name",
                        Value = "RDD-1191 spike console application"
                    }
                }
            };

            try
            {
                var createUserResponse = await cognitoClient.AdminCreateUserAsync(createUserRequest);

                return createUserResponse;
            }
            catch (UsernameExistsException)
            {
                Console.WriteLine($"Username already exists. Please try another unique username");
                return null;
            }
        }
    }
}
