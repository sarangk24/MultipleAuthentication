using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MultipleAuthentication.Controllers
{
    public class PermissionsController : Controller
    {
        // GET: Permissions
        public ActionResult UnauthorizedRequest()
        {
            return View("~/Views/Permissions/AccessDenied.cshtml");
        }

    }
}