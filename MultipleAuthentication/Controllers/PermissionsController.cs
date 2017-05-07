using MultipleAuthentication.Extensions;
using MultipleAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MultipleAuthentication.Controllers
{
    public class PermissionsController : Controller
    {
        [CustomAuthorize(Roles = "Company Administrator")]
        public ActionResult AdminConsent()
        {
            Tenant tenant = ClaimsIdentityExtensions.GetTenant();
            string callBackUrl = "https://localhost:44300/Tenants/Create";
            string url = "https://login.microsoftonline.com/" + tenant.TenantID + "/oauth2/authorize?client_id=" + Startup.clientId + "&response_type=code&redirect_uri="+callBackUrl+"&nonce=1234&resource=https://graph.windows.net&prompt=admin_consent";
            return Redirect(url);
            

        }
        // GET: Permissions
        public ActionResult UnauthorizedRequest()
        {
            return View("~/Views/Permissions/AccessDenied.cshtml");
        }

    }
}