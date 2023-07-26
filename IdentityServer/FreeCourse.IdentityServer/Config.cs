// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
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

        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                    #region Original IdentityResources
		        //new IdentityResources.OpenId(),
                //new IdentityResources.Profile(), 
	            #endregion
                 
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

                    AllowedGrantTypes = GrantTypes.ClientCredentials, // sabitler refresh token olmaz burda zaten üyelik sistmei yok
                    AllowedScopes = {"catalog_fullpermission","photo_stock_fullpermission",IdentityServerConstants.LocalApi.ScopeName}
                }
            };
    }
}