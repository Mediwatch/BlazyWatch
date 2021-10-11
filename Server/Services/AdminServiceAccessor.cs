using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using Server.Services.Interface;

namespace Server.Services
{
    public class AdminServiceAccessor : IAdminServiceAccessor
    {
        public AdminServiceAccessor(IEnumerable<IHostedService> hostedServices)
        {
            foreach (var service in hostedServices)
            {
                if (service is AdminService match)
                {
                    Service = match;
                    break;
                }
            }
        }

        public AdminService Service { get; }
    }
}