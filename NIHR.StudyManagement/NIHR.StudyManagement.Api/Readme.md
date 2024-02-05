# NIHR Study Management API

This project is the API entry point for Study Management. It is an ASP .NET Core application with references to Amazon.Lambda.AspNetCoreServer nuget package, allowing the
application to be deployed as a dotnetcore7 docker lambda function in AWS.

The lambda function is exposed via an API Gateway in AWS.

## Local development
For local development, the API can simply be initialised using standard dotnet run (F5) commands. By default the appsettings.development.json file should configure the runtime
to override jwt token validation i.e. bypassing authentication. This is intended for local development only and should never be used in a production environment.

## To deploy from Visual Studio (using serverless.template)

To deploy your Serverless application, right click the project in Solution Explorer and select *Publish to AWS Lambda*. Most of the default values (aside from AWS credentials)
should default to sandbox entries. Deployments to stable environments such as DEV and beyond will be handled outside of this solution by infrastructure code.
