using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NIHR.StudyManagement.Api.Configuration;
using System.Security.Claims;

namespace NIHR.StudyManagement.Api;

public class Startup
{
    private readonly ILogger<Startup> _logger;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public Startup(IConfiguration configuration,
        IWebHostEnvironment webHostEnvironment)
    {
        Configuration = configuration;

        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Information);
            builder.AddConsole();
        });

        _logger = loggerFactory.CreateLogger<Startup>();

        _webHostEnvironment = webHostEnvironment;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        _logger.LogInformation("Configuring services..");

        var studyManagementApiConfigurationSection = Configuration.GetSection("StudyManagementApi");

        var studyManagementApiSettings = studyManagementApiConfigurationSection.Get<StudyManagementApiSettings>();

        services.AddOptions<StudyManagementApiSettings>().Bind(studyManagementApiConfigurationSection);

        _logger.LogDebug($"Application settings: {System.Text.Json.JsonSerializer.Serialize(studyManagementApiSettings)}");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddCookie()
        .AddJwtBearer(options =>
        {
            options.Authority = studyManagementApiSettings.JwtBearer.Authority;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = studyManagementApiSettings.JwtBearer.ValidateIssuerSigningKey,
                ValidateAudience = studyManagementApiSettings.JwtBearer.ValidateAudience
            };

            // If local settings have a configuration value to override jwt token validation, then add
            // some custom handlers to intercept jwt validation events. Note, this bypasses true authentication
            // and should only be used in a local development environment. Claims can be mocked from the same configuration setting
            if(studyManagementApiSettings.JwtBearer.JwtBearerOverride != null
                && studyManagementApiSettings.JwtBearer.JwtBearerOverride.OverrideEvents)
            {
                var events = ConfigureForLocalDevelopment(studyManagementApiSettings);

                if(events != null)
                {
                    options.Events = events;
                }
            }
        });

        services.AddAuthorization();

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<StudyManagementApiSettings> studyManagementApiSettings)
    {
        if (env.IsDevelopment())
        {
            _logger.LogInformation("Environment is development, initialisating Swagger endpoints.");

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

    /// <summary>
    /// This method is to override jwt token validation so that during local development there are no external dependencies, even
    /// for authentication.
    /// Configuration is controlled via appsettings values.
    /// </summary>
    /// <param name="studyManagementApiSettings"></param>
    /// <returns></returns>
    private JwtBearerEvents? ConfigureForLocalDevelopment(StudyManagementApiSettings studyManagementApiSettings)
    {
        if(studyManagementApiSettings.JwtBearer.JwtBearerOverride == null
            || !studyManagementApiSettings.JwtBearer.JwtBearerOverride.OverrideEvents)
        {
            return null;
        }

        // Log an error if Jwt Bearer token validation is set to override and the environment is Production.
        if (_webHostEnvironment.IsProduction())
        {
            _logger.LogError("Error: Jwt Bearer Override should not be used in Production environment. Ignoring Jwt Bearer Override.");

            return null;
        }

        return new JwtBearerEvents()
        {
            OnMessageReceived = context =>
            {
                var claims = new List<Claim>();

                foreach (var claimConfig in studyManagementApiSettings.JwtBearer.JwtBearerOverride.ClaimsOverride)
                {
                    claims.Add(new Claim(claimConfig.Name, claimConfig.Description));
                }

                context.Principal = new ClaimsPrincipal(
                    new ClaimsIdentity(claims, context.Scheme.Name));

                context.Success();

                return Task.CompletedTask;
            },

            OnChallenge = context =>
            {
                return Task.CompletedTask;
            },

            OnForbidden = context =>
            {
                return Task.CompletedTask;
            },

            OnTokenValidated = context =>
            {
                return Task.CompletedTask;
            },

            OnAuthenticationFailed = context =>
            {
                return Task.CompletedTask;
            }
        };
    }
}