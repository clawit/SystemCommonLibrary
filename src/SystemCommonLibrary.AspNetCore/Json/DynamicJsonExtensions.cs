using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemCommonLibrary.AspNetCore.Json
{
    public static class DynamicJsonExtensions
    {
        public static IMvcBuilder AddDynamicJson(this IMvcBuilder builder)
        {
            return builder.AddMvcOptions(option =>
            {
                option.InputFormatters.Clear();
                option.InputFormatters.Add(new DynamicJsonInputFormatter());
            });
        }
    }
}
