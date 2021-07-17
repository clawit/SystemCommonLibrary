using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using SystemCommonLibrary.AspNetCore.IoC;
using SystemCommonLibrary.IoC.Exceptions;

namespace SystemCommonLibrary.AspNetCore.Auth
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PermissionAttribute : Attribute, IFilterFactory, IOrderedFilter
    {
        public int Order { get; set; }

        bool IFilterFactory.IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var service = serviceProvider.GetRequiredService(typeof(PermissionActionFilter));
            if (service == null)
            {
                throw new UnableResolveDependencyException($"Unable to resolve dependency {typeof(PermissionActionFilter).FullName}");
            }
            var filter = service as IFilterMetadata;
            if (filter == null)
            {
                throw new NotImplementedException($"{typeof(PermissionActionFilter).FullName} not implement IFilterMetadata");
            }
            DependencyInjection.Resolve(serviceProvider, filter);
            return filter;
        }
    }
}
