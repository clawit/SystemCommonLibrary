using System;
using SystemCommonLibrary.Auth;

namespace SystemCommonLibrary.AspNetCore.Auth
{
    public static class PrvlgAuth
    {
        /// <summary>
        /// 权限授权
        /// </summary>
        /// < returns ></ returns >
        public static bool Authorize(string privilege, Func<PrivilegeIdentity, bool> checkPrvlg)
        {
            var prvlg = PrvlgReader.Read(privilege);

            return checkPrvlg(prvlg);
        }

    }
}
