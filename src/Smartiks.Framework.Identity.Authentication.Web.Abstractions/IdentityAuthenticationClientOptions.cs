namespace Smartiks.Framework.Identity.Authentication.Web.Abstractions
{
    public class IdentityAuthenticationClientOptions
    {
        /// <summary>
        /// Base-address of the token issuer
        /// </summary>
        public string Authority { get; set; }

        /// <summary>
        /// Name of the API resource used for authentication against introspection endpoint
        /// </summary>
        public string ApiName { get; set; }

        /// <summary>
        /// Secret used for authentication against introspection endpoint
        /// </summary>
        public string ApiSecret { get; set; }
    }
}
