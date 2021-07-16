using System;
using SystemCommonLibrary.Auth;
using SystemCommonLibrary.Enums;
using SystemCommonLibrary.Network;
using SystemCommonLibrary.Network.Valid;

namespace SystemCommonLibrary.AspNetCore.Auth
{
    public static class TokenAuth
    {
        /// <summary>
        /// 授权
        /// </summary>
        /// <param name = "id" > 用户编号 </ param >
        /// < param name="token">用户token</param>
        /// <param name = "actionContext" ></ param >
        /// < returns ></ returns >
        public static bool Authorize(string authorization, string agents, Func<TokenAuthIdentity, HttpClientType, bool> checkToken)
        {
            try
            {
                if (!authorization.StartsWith(AuthConst.AuthPrefix))
                {
                    return false;
                }
                var auth = AuthReader.Read(authorization);

                if (auth == null)
                    return false;
                else
                {
                    if (auth.Id == 0 || !StringValid.IsEmpty(auth.Token))
                        return false;
                    else
                        return checkToken(auth, HttpClientReader.Read(agents));
                }
            }
            catch 
            {
                return false;
            }
        }

    }
}
