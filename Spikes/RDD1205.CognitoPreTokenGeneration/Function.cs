using Amazon.Lambda.CognitoEvents;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace RDD1205.CognitoPreTokenGeneration;

public class Function
{

    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input">The event for the Lambda function handler to process.</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public CognitoPreTokenGenerationV2Event FunctionHandler(CognitoPreTokenGenerationV2Event input, ILambdaContext context)
    {
        var response = new CognitoPreTokenGenerationV2Response
        {
            ClaimsAndScopeOverrideDetails = new ClaimsAndScopeOverrideDetails
            {
                AccessTokenGeneration = new AccessTokenGeneration
                {
                    ClaimsToAddOrOverride = new Dictionary<string, string>
                    {
                        {"RDD-1205", "If you see me, I'm a modified access token" }
                    }
                }
            }
        };

        input.Response = response;

        return input;
    }
}
