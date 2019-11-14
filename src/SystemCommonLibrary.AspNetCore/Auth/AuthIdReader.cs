using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemCommonLibrary.AspNetCore.Auth
{
    public static class AuthIdReader
    {
        public static int Read(HttpRequest request)
        {
            if (request == null || request.Headers.Count == 0 
                || request.Headers["Authorization"].Count == 0 )
            {
                return 0;
            }
            var authorization = request.Headers["Authorization"].ToString();
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
