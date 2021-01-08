using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;
using SystemCommonLibrary.AspNetCore.IoC;
using SystemCommonLibrary.IoC.Extensions;

namespace SystemCommonLibrary.AspNetCore.Ioc
{
    public static class AutowiredServiceExtensions
    {
        public static void AddAutowiredService(this IServiceCollection services, IEnumerable<string> assemblies)
        {
            services.Replace(ServiceDescriptor.Transient<IViewComponentActivator, AutowiredViewComponentActivator>());
            services.Replace(ServiceDescriptor.Transient<IControllerActivator, AutowiredControllerActivator>());
            //use auto dependency injection
            services.AutoRegisterDependency(assemblies);
        }
    }
}
