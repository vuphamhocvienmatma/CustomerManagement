using Bussiness.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Configure
{
    public class ConfigureServiceInjection
    {
        public static class ConfigureServices
        {
            public static void ConfigureServiceInjection(IServiceCollection services)
            {
                services.AddTransient<ICustomerService, CustomerService>();
                services.BuildServiceProvider();
            }
        }
    }
}
