using System;
using System.Collections.Generic;
using System.Text;

namespace SystemCommonLibrary.AspNetCore.Auth
{
    public static class PrvlgAuth
    {
        /// <summary>
        /// 权限授权
        /// </summary>
        /// < returns ></ returns >
        public static bool Authorize(string privilege, Func<string, string, bool> checkPrvlg)
        {
            var prvlg = PrvlgReader.Read(privilege);

            if (prvlg == null || string.IsNullOrEmpty(prvlg.Name) || string.IsNullOrEmpty(prvlg.Prvlg))
                return false;
            else
                return checkPrvlg(prvlg.Name, prvlg.Prvlg);

        }

    }
}
