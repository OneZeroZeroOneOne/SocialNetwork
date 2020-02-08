using Microsoft.AspNetCore.Authorization;

namespace SocialNetwork.Security.Extensions
{
    public static class AuthPolicy
    {
        public static void ConfigurePolicy(this AuthorizationOptions config)
        {
            config.AddPolicy("ConfirmedUser",
                policy => policy.RequireRole("Member", "Admin"));
            config.AddPolicy("AdminUser", 
                policy => policy.RequireRole("Admin"));
        }
    }
}
