using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

namespace MultipleAuthentication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            try
            {
                ConfigureAuth(app);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw;
            }
            
        }
    }
}
