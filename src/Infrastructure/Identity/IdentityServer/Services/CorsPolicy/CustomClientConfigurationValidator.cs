using System.Threading.Tasks;
using IdentityServer4.Configuration;
using IdentityServer4.Validation;

namespace IdentityServer.Services.CorsPolicy;

public class CustomClientConfigurationValidator(IdentityServerOptions options)
    : DefaultClientConfigurationValidator(options)
{
    protected override Task ValidateAllowedCorsOriginsAsync(ClientConfigurationValidationContext context)
    {
        if (context.Client.AllowedCorsOrigins.Count > 0)
        {
            foreach (var origin in context.Client.AllowedCorsOrigins)
            {
                if (string.IsNullOrEmpty(origin))
                    context.SetError($"AllowedCorsOrigins contains invalid origin. There is an empty value.");
                else if (origin.EndsWith('/') || CorsPolicyExtensions.CreateDomainUri(origin).AbsolutePath != "/")
                    context.SetError($"AllowedCorsOrigins contains invalid origin: {origin}");
            }
        }

        return Task.CompletedTask;
    }
}