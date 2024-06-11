using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.Extensions.DependencyInjection;
using NIHR.NSIP.Interoperability.Shared.Authorizer;
using NIHR.NSIP.Interoperability.Shared.Authorizer.Abstractions;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace NIHR.StudyManagement.Api.Authorizer;

public class Function
{
    private readonly string applicationName = "NIHR.StudyManagement.Api.Authorizer";
    private ServiceProvider serviceProvider;

    public Function()
    {
        this.ConfigureServices();
    }

    /// <summary>
    /// This method is the entry point for an AWS Lambda Authorizer request APIGatewayCustomAuthorizerRequest.
    /// It will invoke functionality to validate the client token as passed in the API request Authorization header.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public APIGatewayCustomAuthorizerResponse FunctionHandler(APIGatewayCustomAuthorizerRequest request, ILambdaContext context)
    {
        try
        {
            context.Logger.LogLine($"{this.applicationName}: Validating client token: {request.AuthorizationToken}");

            var apiGatewayAuthorizer = this.serviceProvider.GetService<IAPIGatewayAuthorizer>();

            return apiGatewayAuthorizer.ValidateTokenAndGeneratePolicyDocument(request, context);
        }
        catch (Exception ex)
        {
            context.Logger.LogLine($"Exception during validation of token. {ex.Message}");

            throw;
        }
    }

    /// <summary>
    /// Set up IOC for lamba function.
    /// </summary>
    private void ConfigureServices()
    {
        var serviceCollection = new ServiceCollection();

        // Set up to use the JWT token validation from public keys implementation.
        ServiceRegistrationExtension.RegisterLambdaAuthorizer(serviceCollection, TokenValidationImplementationType.JwtPublicKey);

        this.serviceProvider = serviceCollection.BuildServiceProvider();
    }
}
