using MultipleAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleAuthentication.Interfaces
{
    interface ITenantProvider
    {
         Tenant GetTenantDetails();
    }
}
