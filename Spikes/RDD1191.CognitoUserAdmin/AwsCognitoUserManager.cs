using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;

namespace RDD1191.CreateCognitoUser
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            Console.WriteLine("Please enter Cognito user pool ID.");

            var userPoolId = Console.ReadLine();

            if (string.IsNullOrEmpty(userPoolId))
            {
                Console.WriteLine("User pool ID must be specified.");
                return;
            }

            var awsCognitoUserManager = new AwsCognitoUserManager(userPoolId);

            Console.WriteLine("Please enter the username of the Cognito User you wish to view.");

            var username = Console.ReadLine();

            if(string.IsNullOrEmpty(username))
            {
                Console.WriteLine("Username must be specified.");
                return;
            }

            await awsCognitoUserManager.GetUser(username);
        }
    }

    public class AwsCognitoUserManager
    {
        const string cognitoUserPoolId = "eu-west-2_cAq5FnIi4";
        private string _userPoolId;

        public AwsCognitoUserManager(string userPoolId)
        {
            _userPoolId = userPoolId ?? cognitoUserPoolId;
        }

        public async Task GetUser(string username)
        {
            var cancellationTokenSource = new CancellationTokenSource();

            cancellationTokenSource.CancelAfter(5000);

            // No credentials passed in here. This assumes credentials of local Visual Studio user .\aws\credentials file.
            var chain = new CredentialProfileStoreChain();
            var result = chain.TryGetAWSCredentials("Default", out var credentials);

            AmazonCognitoIdentityProviderClient cognitoClient = new AmazonCognitoIdentityProviderClient(credentials, RegionEndpoint.EUWest2);

            var getUserRequest = new AdminGetUserRequest {
                Username = username,
                UserPoolId = _userPoolId
            };

            var getUserResponse = await cognitoClient.AdminGetUserAsync(getUserRequest, cancellationTokenSource.Token);

            if(getUserResponse.HttpStatusCode != System.Net.HttpStatusCode.OK) {
                Console.WriteLine($"Failed to read user profile");
                return;
            }

            Console.WriteLine(getUserResponse.Username);
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(getUserResponse.UserAttributes));
        }

        public async Task CreateUser(string username)
        {
            var sharedFile = new SharedCredentialsFile();
            sharedFile.TryGetProfile("default", out var profile);
            AWSCredentialsFactory.TryGetAWSCredentials(profile, sharedFile, out var credentials);

            AmazonCognitoIdentityProviderClient cognitoClient = new AmazonCognitoIdentityProviderClient(credentials, RegionEndpoint.EUWest2);



            var createUserRequest = new AdminCreateUserRequest
            {
                Username = username,
                UserPoolId = _userPoolId,
                MessageAction = MessageActionType.SUPPRESS,
                UserAttributes = new List<AttributeType>
                {
                    new AttributeType
                    {
                        Name = "source",
                        Value = "RDD-1191 spike console application"
                    }
                }
            };

            var createUserResponse = await cognitoClient.AdminCreateUserAsync(createUserRequest);


        }
    }
}
