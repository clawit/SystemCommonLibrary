using Microsoft.AspNetCore.Http;
using SystemCommonLibrary.Auth;
using SystemCommonLibrary.Valid;

namespace SystemCommonLibrary.AspNetCore.Auth
{
    public static class AuthIdReader
    {
        public static int Read(HttpRequest request)
        {
            if (request == null || request.Headers.Count == 0 
                || request.Headers[AuthConst.AuthKey].Count == 0
                || request.Cookies[AuthConst.AuthKey].IsEmpty())
            {
                return 0;
            }
            var authorization = request.Headers[AuthConst.AuthKey].ToString();
            if (authorization.IsEmpty())
            {
                //如果header中没有读到尝试从cookie中读取
                authorization = request.Cookies[AuthConst.AuthKey];
            }

            if (authorization.IsEmpty() || !authorization.StartsWith(AuthConst.AuthPrefix))
            {
                return 0;
            }

            var identity = AuthReader.Read(authorization);
            if (identity == null)
                return 0;
            else
                return identity.Id;
        }

        public static bool Read(HttpRequest request, int reqId)
        {
            return Read(request) == reqId;
        }

        public static bool Read(HttpRequest request, int reqId, ref int id)
        {
            id = Read(request);
            return id == reqId;
        }

    }
}
