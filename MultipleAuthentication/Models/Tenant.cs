using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MultipleAuthentication.Models
{
    public class Tenant
    {
        public string TenantID { get; set; }
        public string TenantDomain { get; set; }

        public string TenantName { get; set; }


        public Tenant(string tenantId, string tenantDomain, string tenantName)
        {
            TenantID = tenantId;
            TenantDomain = tenantDomain;
            TenantName = tenantName;

        }
    }
}