using Microsoft.AspNetCore.Http;
using System.Linq;

namespace SystemCommonLibrary.Network
{
    public class Http
	{
		public static string GetIp (HttpRequest request)
		{
            string ip = "0.0.0.0";
            if (request == null || request.Headers.Count == 0
                || request.Headers["X-Forwarded-For"].Count == 0)
            {
                return ip;
            }
            else
            {
                ip = request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (string.IsNullOrEmpty(ip))
                    ip = "0.0.0.0";
            }

            return ip;
        }
    }
}
