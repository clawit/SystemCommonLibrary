using Microsoft.AspNetCore.Mvc.Formatters;
using System.IO;
using System.Threading.Tasks;
using SystemCommonLibrary.Json;

namespace SystemCommonLibrary.AspNetCore.Json
{
    public sealed class DynamicJsonInputFormatter : IInputFormatter
    {
        public bool CanRead(InputFormatterContext context) => context.HttpContext.Request.ContentType.StartsWith("application/json");

        public async Task<InputFormatterResult> ReadAsync(InputFormatterContext context)
        {
            var request = context.HttpContext.Request;

            if (request.Body.CanSeek && request.Body.Length == 0)
                return await InputFormatterResult.NoValueAsync();

            var reader = new StreamReader(request.Body);
            string json = await reader.ReadToEndAsync();

            var result = DynamicJson.Parse(json);
            return await InputFormatterResult.SuccessAsync(result);
        }
    }
}
