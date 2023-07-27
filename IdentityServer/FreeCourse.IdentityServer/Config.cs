// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using static IdentityServer4.Models.IdentityResources;

namespace FreeCourse.IdentityServer
{
    public static class Config
    {
        #region Audiences

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("resource_catalog") { Scopes={"catalog_fullpermission"}},
                new ApiResource("resource_photo_stock") { Scopes={"photo_stock_fullpermission"}},
                new ApiResource(IdentityServerConstants.LocalApi.ScopeName ),
            };

        #endregion

        /// <summary>
        /// Resource Owner Credential ile bir token alındığında clientlar, kullanıcı hakkında hangi bilgilere sahip olacak.
        /// </summary>
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                    #region Original IdentityResources
		        //new IdentityResources.OpenId(),
                //new IdentityResources.Profile(), 
	            #endregion
                 
                       new IdentityResources.OpenId(), // olmak zorunda jwt.sub , kullanıcı Id claimi
                       new IdentityResources.Email(), // email claimi
                       new IdentityResources.Profile(),
                       new IdentityResource(){Name="roles", DisplayName="Roles", Description = "User roles", UserClaims=new []{ "role"} } // role adındaki claimime maplensin

                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                #region Original Scopes
                //new ApiScope("scope1"),
                //new ApiScope("scope2"), 
	            #endregion
            new ApiScope("catalog_fullpermission","Full access for Catalog API" ),
            new ApiScope("photo_stock_fullpermission","Full access for Photo Stock API" ),
            new ApiScope(IdentityServerConstants.LocalApi.ScopeName,"Full access for IdentityServer API" ),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                #region Original Clients
		                //// m2m client credentials flow client
                //new Client
                //{
                //    ClientId = "m2m.client",
                //    ClientName = "Client Credentials Client",

                //    AllowedGrantTypes = GrantTypes.ClientCredentials,
                //    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                //    AllowedScopes = { "scope1" }
                //},

                //// interactive client using code flow + pkce
                //new Client
                //{
                //    ClientId = "interactive",
                //    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                //    AllowedGrantTypes = GrantTypes.Code,

                //    RedirectUris = { "https://localhost:44300/signin-oidc" },
                //    FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                //    PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                //    AllowOfflineAccess = true,
                //    AllowedScopes = { "openid", "profile", "scope2" }
                //}, 
	            #endregion
           
                new Client
                {
                    ClientName="Asp.Net Core MVC",
                    ClientId="WebMvcClient",
                    ClientSecrets= { new Secret("secret".Sha256())},

                    AllowedGrantTypes = GrantTypes.ClientCredentials, // refresh token olmaz burda zaten üyelik sistmei yok
                    AllowedScopes = {"catalog_fullpermission","photo_stock_fullpermission",IdentityServerConstants.LocalApi.ScopeName}
                },
                
                new Client
                {
                    ClientName="Asp.Net Core MVC For User",
                    ClientId="WebMvcClientForUser",
                    ClientSecrets= { new Secret("secret".Sha256())},

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword, // üyelik sistemi, refresh token var
                    AllowedScopes = 
                      {
                          IdentityServerConstants.StandardScopes.OpenId,
                          IdentityServerConstants.StandardScopes.Email,
                          IdentityServerConstants.StandardScopes.Profile,
                          IdentityServerConstants.StandardScopes.OfflineAccess, // refersh token : kullanıcı offline olsa dahi elimdeki refresh token ile yeni bir AccessToken alabilirim. İsmi o nedenle offlineAccess, bu olmazsa refresh tok en olmaz, refresh token olmazsa ye accesstoken süresi uzatılr ki bu iyi değil, ya da kullanıcı her accesstoken ömrü dolduğunda kullanıcıyı login ekranıa döndürmem gerekir.
                          "roles"
                      },

                    AccessTokenLifetime = 1* 60 * 60, // 1 saat

                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddDays(60) - DateTime.Now).TotalSeconds, // 60 gün
                    RefreshTokenUsage = TokenUsage.ReUse
                }
            };
    }
}