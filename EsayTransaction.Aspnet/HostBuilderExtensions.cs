using AspectCore.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsayTransaction.Aspnet
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseEsayTransactionDependencyInjection(this IHostBuilder host)
        {
            return host.UseServiceProviderFactory(new DynamicProxyServiceProviderFactory());
        }
    }
}
