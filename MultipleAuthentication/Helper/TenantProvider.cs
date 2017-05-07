using MultipleAuthentication.Extensions;
using MultipleAuthentication.Interfaces;
using MultipleAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MultipleAuthentication.Helper
{
    public class TenantProvider:ITenantProvider
    {
        public Tenant GetTenantDetails()
        {
            var allClaims = ((System.Security.Claims.ClaimsIdentity)HttpContext.Current.GetOwinContext().Request.User.Identity).Claims;
            Models.Tenant tenantModel = allClaims.GetTenantDetails();
            return tenantModel;

        }
    }
}