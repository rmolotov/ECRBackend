namespace Shared.Api;

public static class Constants
{
    public static class Authentication
    {
        public static class AuthorizationPolicies
        {
            public const string ApiScopePolicy = "ApiScopePolicy";
            public const string UserScopePolicy = "UserScopePolicy";

            public const string ApiScope = "ECR.Api";
            public const string UserScope = "ECR.Web";
        }
    }
}
