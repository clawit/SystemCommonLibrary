using System;
using System.Web;

namespace SystemCommonLibrary.Network
{
	public class Http
	{
		public static string GetIP (HttpRequest request)
		{
			if (request.ServerVariables ["HTTP_VIA"] != null) // using proxy
			{
				return request.ServerVariables ["HTTP_X_FORWARDED_FOR"].ToString ();  // Return real client IP.
			} else// not using proxy or can't get the Client IP
			  {
				return request.ServerVariables ["REMOTE_ADDR"].ToString (); //While it can't get the Client IP, it will return proxy IP.
			}
		}
	}
}
