using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editlio.Domain
{
    public static class BusinessServiceRegistration
    {
        public static IServiceCollection AddBusinessLayer(this IServiceCollection services)
        {
            // Business katmanında kullanılan servisleri burada ekleyin


            return services;
        }
    }
}
