using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using SystemCommonLibrary.Auth;
using SystemCommonLibrary.Enums;

namespace SystemCommonLibrary.AspNetCore.Auth
{
    public static class PermissionExtensions
    {
        public static IServiceCollection AddPermission(this IServiceCollection services)
        {
            services.AddScoped<PermissionActionFilter>();
            return services;
        }

        public static IApplicationBuilder UsePermission(this IApplicationBuilder builder,
                Func<PermissionActionContext, TokenAuthIdentity, HttpClientType, Task<bool>> checkPermission)
        {
            AuthConst.CheckPermission = checkPermission;

            return builder;
        }

    }
}
