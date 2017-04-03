using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MultipleAuthentication.Constants
{
    public class ActiveDirectoryConstants
    {
        public static string GetCurrentApplicationUrl {
            get {
                string baseUrl = "https://localhost:44300/";
                return baseUrl;
            }
        }

         public const string ResourceUrl = "https://graph.windows.net";
        public const string AuthString = "https://login.windows.net/";
    }
}