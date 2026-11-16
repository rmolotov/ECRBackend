using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Shared.Api.Endpoints;

public static class DiagnosticEndpointsExtensions
{
    extension(WebApplication app)
    {
        public WebApplication UseDiagnosticEndpoints()
        {
            app.MapHealthChecks();

            return app;
        }

        private void MapHealthChecks()
        {
            app.MapHealthChecks(
                "/diagnostic/healthcheck",
                new HealthCheckOptions
                {
                    ResultStatusCodes =
                    {
                        [HealthStatus.Healthy]   = StatusCodes.Status200OK,
                        [HealthStatus.Degraded]  = StatusCodes.Status200OK,
                        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                    }
                });
        }
    }
}