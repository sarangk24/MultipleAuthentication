using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MultipleAuthentication.Constants;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MultipleAuthentication.Extensions
{
    public class CustomAuthorize: AuthorizeAttribute
    {
        bool authorize = false;
        // Entities context = new Entities(); // my entity  
        private  string[] allowedroles;
        public CustomAuthorize(params string[] Roles)
        {
            
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (this.Roles.Contains(","))
                this.allowedroles = this.Roles.Split(',');
            else
                this.allowedroles = new String[] { this.Roles };

            
            var allClaims = ((System.Security.Claims.ClaimsIdentity)HttpContext.Current.GetOwinContext().Request.User.Identity).Claims;
            Models.Tenant tenant = allClaims.GetTenantDetails();

            var authContext = new AuthenticationContext(ActiveDirectoryConstants.AuthString  + tenant.TenantID);
            if (authContext.TokenCache.ReadItems().Count() > 0)
                authContext = new AuthenticationContext(authContext.TokenCache.ReadItems().First().Authority);

            var authResult = authContext.AcquireTokenAsync(ActiveDirectoryConstants.ResourceUrl, new ClientCredential(Startup.clientId, new Startup().appKey));

            var client = new System.Net.Http.HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://graph.windows.net/"+tenant.TenantID+"/users/"+HttpContext.Current.GetOwinContext().Request.User.Identity.Name+"/memberOf?api-version=1.6");
            //var request = new HttpRequestMessage(HttpMethod.Get, "https://graph.windows.net/" + tenant.TenantID + "/users/" + HttpContext.Current.GetOwinContext().Request.User.Identity.Name + "/memberOf?api-version=1.6");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.AccessToken);
            var response = client.SendAsync(request);

            var content = response.Result.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(content.Result);
            var data=json.Last.Values()["displayName"];
            var numerator=data.GetEnumerator();
            while (numerator.MoveNext())
            {
                if (this.allowedroles.Contains(numerator.Current.ToString()))
                {
                    authorize = true;
                }
            }

            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var key = CreateKey(filterContext);
            var isCached = HttpRuntime.Cache.Get(key);

            if (isCached != null)
            {
                if (authorize == false)
                    HttpContext.Current.Response.Redirect("~/Views/Shared/Error.cshtml");
            }

            HttpRuntime.Cache.Insert(key, true);
            if (authorize == false)
                filterContext.Result = new RedirectToRouteResult(
                               new RouteValueDictionary
                               {
                                   { "action", "Index" },
                                   { "controller", "Sections" }
                               });

        }

        private string CreateKey(AuthorizationContext context)
        {
            // Or create some other unique key that allows you to identify 
            // the same request
            return
                context.RequestContext.HttpContext.User.Identity.Name + "|" +
                context.RequestContext.HttpContext.Request.Url.AbsoluteUri;
        }

    }
}