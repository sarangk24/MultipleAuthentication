using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MultipleAuthentication.Interfaces
{
    //public class AzureAuthenticationProvider : Microsoft.Graph.IAuthenticationProvider
    //{
    //    public async Task AuthenticateRequestAsync(HttpRequestMessage request)
    //    {
    //        string clientId = ConfigurationManager.AppSettings["ClientId"];
    //        string clientSecret = ConfigurationManager.AppSettings["ClientSecret"];
    //        string authority = ConfigurationManager.AppSettings["AuthorityUrl"];

    //        var authContext = new AuthenticationContext(authority);

    //        var creds = new ClientCredential(clientId, clientSecret);

    //        Console.WriteLine("before: get token");
    //        var authResult =
    //await authContext.AcquireTokenAsync("https://graph.microsoft.com/", creds);
    //        Console.WriteLine("after: get token {0}", authResult.AccessToken);

    //        request.Headers.Add("Authorization", "Bearer " + authResult.AccessToken);
    //    }
    }