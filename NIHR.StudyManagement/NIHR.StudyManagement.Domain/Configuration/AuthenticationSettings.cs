

namespace NIHR.StudyManagement.Domain.Configuration
{
    public class AuthenticationSettings
    {
        public string ClientId { get; set; } = "";

        public string ClientSecret { get; set; } = "";

        public string MetadataAddress { get; set; } = "";

        public string ResponseType { get; set; } = "";

        public string Issuer { get; set; } = "";

        public string Domain { get; set; } = "";

        public string RedirectUrl { get; set; } = "";

        public string NihrIdentityProviderName { get; set; } = "";

        public string NihrIdgLoginUrl
        {
            get
            {
                return $"{Domain}/oauth2/authorize?identity_provider={NihrIdentityProviderName}&client_id={ClientId}&response_type={ResponseType}&scope=openid&redirect_uri=https%3A%2F%2Flocalhost%3A7094%2Fsignin-oidc";
            }
        }

    }
}
