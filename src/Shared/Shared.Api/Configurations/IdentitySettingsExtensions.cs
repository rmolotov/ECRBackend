namespace Shared.Api.Configurations;

public static class IdentitySettingsExtensions
{
    public static IdentitySettings GetIdentitySettings(this IConfiguration configuration) =>
        configuration
            .GetSection(IdentitySettings.SectionName)
            .Get<IdentitySettings>()
        ?? throw new InvalidOperationException($"Configuration section '{IdentitySettings.SectionName}' is missing");
}
