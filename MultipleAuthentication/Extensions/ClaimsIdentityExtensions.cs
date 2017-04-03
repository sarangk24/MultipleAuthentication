using Microsoft.Azure.ActiveDirectory.GraphClient;
using MultipleAuthentication.Interfaces;
using MultipleAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using Microsoft.Graph;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MultipleAuthentication.Constants;
using System.Net.Http.Headers;

namespace MultipleAuthentication.Extensions
{
    public static class ClaimsIdentityExtensions
    {
         
        public static Tenant GetTenantDetails(this IEnumerable<System.Security.Claims.Claim> claims)
        {
            string domain = string.Empty;
            //there is no identityprovider for test users in onmicrosoft domain
            var claim = claims.FirstOrDefault(c => c.Type.Equals("http://schemas.microsoft.com/identity/claims/identityprovider"));
            if (claim != null)
                domain = claim.Value;
            else
            {
                claim=claims.FirstOrDefault(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"));
                domain = claim.Value.Substring(claim.Value.IndexOf('@')+1);
            }
            var tenantID = claims.FirstOrDefault(c => c.Type.Equals("iss")).Value.TrimEnd('/');
            tenantID = tenantID.Substring(tenantID.LastIndexOf('/') + 1);
            var tenantName = domain.Substring(0, domain.LastIndexOf('.'));
            Tenant tenant = new Tenant(tenantID, domain, tenantName);

            //var allUsers = users.Result.CurrentPage.Any();
            return tenant;
        }

        public static IEnumerable<string> GetRoles(this System.Security.Principal.IPrincipal user)
        {
            // We will use Graph Api to fetch users and their roles
            return null;
        }
    }


    
}