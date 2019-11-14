using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SystemCommonLibrary.Network.Valid;

namespace SystemCommonLibrary.Network
{
    public static class HttpClientReader
    {
        public static HttpClientType Read(string agents)
        {
            if (!NullValid.CheckNull(agents).Correct
                || !StringValid.CheckEmpty(agents).Correct)
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
                if (!NullValid.CheckNull(agent.Product).Correct
                    || !StringValid.CheckEmpty(agent.Product.Name).Correct)
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
