namespace NIHR.StudyManagement.Api.Configuration
{
    public class JwtBearerSettings
    {
        public string Authority { get; set; }

        public bool ValidateAudience { get; set; }

        public bool ValidateIssuerSigningKey { get; set; }

        public JwtBearerOverrideSettings JwtBearerOverride { get; set; }

        public JwtBearerSettings()
        {
            JwtBearerOverride = new JwtBearerOverrideSettings();
            Authority = string.Empty;
        }
    }
}
