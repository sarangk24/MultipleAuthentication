using MultipleAuthentication.ErrorHandlers;
using System.Web;
using System.Web.Mvc;

namespace MultipleAuthentication
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ApplicationErrorHandler());
        }
    }
}
