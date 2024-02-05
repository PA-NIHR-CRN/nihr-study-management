namespace NIHR.StudyManagement.Api.Configuration
{
    public class JwtBearerOverrideSettings
    {
        /// <summary>
        /// Gets or sets a property that, when true, will override and bypass Jwt token validation
        /// allowing for local development without dependency on authentication server.
        /// </summary>
        public bool OverrideEvents { get; set; }

        /// <summary>
        /// Gets or sets a collection of claims that when OverrideEvents is true, then the claims from this
        /// collection will be added to the forced authenticated identity.
        /// </summary>
        public List<OverrideClaimsSetting> ClaimsOverride { get; set; }

        public JwtBearerOverrideSettings()
        {
            ClaimsOverride = new List<OverrideClaimsSetting>();
        }
    }
}
