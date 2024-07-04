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

            while (true)
            {
                Console.WriteLine("Would you like to CREATE or READ a user. Type CREATE or READ. Press CTRL-C to abort.");
                var userInput = Console.ReadLine();

                if (!string.IsNullOrEmpty(userInput) && userInput.Equals("CREATE", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Please enter the username of the Cognito User you wish to create");

                    var username = Console.ReadLine();

                    if (string.IsNullOrEmpty(username))
                    {
                        Console.WriteLine("Username must be specified.");
                        continue;
                    }

                    var createUserResponse = await awsCognitoUserManager.CreateUser(username);

                    if(createUserResponse == null)
                    {
                        continue;
                    }

                    Console.WriteLine();
                    Console.WriteLine($"User created with username: {createUserResponse.User.Username} and the following attributes");

                    var heading = String.Format("{0, 20} | {1, 5}", "Name", "Value");

                    Console.WriteLine();
                    Console.WriteLine(heading);
                    Console.WriteLine("--------------------------------------------------------------------");

                    foreach (var userAttribute in createUserResponse.User.Attributes)
                    {
                        var row = String.Format("{0, 20} | {1, 5}", userAttribute.Name, userAttribute.Value);
                        Console.WriteLine(row);
                    }

                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                }
                else if (!string.IsNullOrEmpty(userInput) && userInput.Equals("READ", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Please enter the username of the Cognito User you wish to view.");

                    var username = Console.ReadLine();

                    if (string.IsNullOrEmpty(username))
                    {
                        Console.WriteLine("Username must be specified.");
                        return;
                    }

                    var getUserResponse = await awsCognitoUserManager.GetUser(username);

                    if (getUserResponse == null) {
                        Console.WriteLine($"Could not find user with username '{username}'");
                        continue;
                    }

                    Console.WriteLine();
                    Console.WriteLine($"Found user with username: {getUserResponse.Username}. User has the following attributes");

                    var heading = String.Format("{0, 20} | {1, 5}", "Name", "Value");

                    Console.WriteLine();
                    Console.WriteLine(heading);
                    Console.WriteLine("--------------------------------------------------------------------");

                    foreach (var userAttribute in getUserResponse.UserAttributes)
                    {
                        var row = String.Format("{0, 20} | {1, 5}", userAttribute.Name, userAttribute.Value);
                        Console.WriteLine(row);
                    }

                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Invalid action");
                }
            }
        }
    }
}
