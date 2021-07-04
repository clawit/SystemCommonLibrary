using System.Net.Http;
using SystemCommonLibrary.Enums;
using SystemCommonLibrary.Network.Valid;

namespace SystemCommonLibrary.Network
{
    public static class HttpClientReader
    {
        public static HttpClientType Read(string agents)
        {
            if (!NullValid.IsNull(agents) || !StringValid.IsEmpty(agents))
            {
                return HttpClientType.Web;
            }
            else if (agents.ToLower().Contains("android"))
            {
                return HttpClientType.AndroidNative;
            }
            else if (agents.ToLower().Contains("ios") || agents.ToLower().Contains("iphone"))
            {
                return HttpClientType.IosNative;
            }
            else if (agents.ToLower().Contains("wap"))
            {
                return HttpClientType.Wap;
            }
            else
            {
                return HttpClientType.Web;
            }
        }

        public static HttpClientType Read(HttpRequestMessage request)
        {
            var agents = request.Headers.UserAgent;

            foreach (var agent in agents)
            {
                if (!NullValid.IsNull(agent.Product) || !StringValid.IsEmpty(agent.Product.Name))
                {
                    continue;
                }
                else if (agent.Product.Name.ToLower().Contains("android"))
                {
                    return HttpClientType.AndroidNative;
                }
                else if (agent.Product.Name.ToLower().Contains("ios") || agent.Product.Name.ToLower().Contains("iphone"))
                {
                    return HttpClientType.IosNative;
                }
                else if (agent.Product.Name.ToLower().Contains("wap"))
                {
                    return HttpClientType.Wap;
                }
            }

            return HttpClientType.Web;
        }

    }
}
