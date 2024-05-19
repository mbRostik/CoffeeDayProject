using Duende.IdentityServer.Models;

namespace IdentityServer.WebApi
{
    public class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
           new IdentityResource[]
           {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
           };

        public static IEnumerable<ApiScope> ApiScopes =>
           new ApiScope[]
           {
               new ApiScope("Users.WebApi.Scope"),
               new ApiScope("ContactUs.WebApi.Scope"),
               new ApiScope("Menu.WebApi.Scope")


           };

        public static IEnumerable<ApiResource> ApiResources => new[] {
            new ApiResource("Users.WebApi")
             {
                 Scopes=new List<string>{ "Users.WebApi.Scope"},
                 ApiSecrets=new List<Secret>{new Secret("Users.WebApi.Secret".Sha256())},
             },
            new ApiResource("Menu.WebApi")
             {
                 Scopes=new List<string>{ "Menu.WebApi.Scope"},
                 ApiSecrets=new List<Secret>{new Secret("Menu.WebApi.Secret".Sha256())},
             },
            new ApiResource("ContactUs.WebApi")
             {
                 Scopes=new List<string>{ "ContactUs.WebApi.Scope"},
                 ApiSecrets=new List<Secret>{new Secret("ContactUs.WebApi.Secret".Sha256())},
             }
        };

        public static IEnumerable<Client> Clients =>
           new Client[]
           {
                new Client
                {
                    ClientId = "interactive",
                    ClientSecrets = {new Secret("OnlyUserKnowsThisSecret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = { "https://localhost:5173/signin-oidc" },
                    FrontChannelLogoutUri="https://localhost:5173/signout-oidc",
                    PostLogoutRedirectUris={ "https://localhost:5173/signout-callback-oidc" },
                    AllowOfflineAccess = true,
                    AllowedScopes = 
                    {"openid", "profile", "Users.WebApi.Scope", "ContactUs.WebApi.Scope", "Menu.WebApi.Scope"},
                    RequireConsent = true,
                    RequirePkce=true,
                    AllowPlainTextPkce=true,
                    AllowedCorsOrigins = { "https://localhost:5173" }
                }
           };
    }
}