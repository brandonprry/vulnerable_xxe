
using System;
using System.Web;
using System.Web.UI;
using System.Xml;
using System.IO;

namespace vulnerable_xxe
{
	public class VulnerableHandler : System.Web.IHttpHandler
	{
		public virtual bool IsReusable {
			get {
				return false;
			}
		}

		public virtual void ProcessRequest (HttpContext context)
		{
			if (context.Request.RequestType != "POST") {
				context.Response.Write ("Needs to be a POST");
				return;
			}

			if (string.IsNullOrEmpty (context.Request ["XML"])) {
				context.Response.Write("Needs an XML Param");
				return;
			}

			byte[] xmlBytes = Convert.FromBase64String(context.Request["XML"]);

			XmlReaderSettings settings = new XmlReaderSettings();
			settings.ProhibitDtd = false;

			XmlDocument doc = new XmlDocument();
			doc.Load(XmlReader.Create(new MemoryStream(xmlBytes), settings));

			context.Response.Write(doc.LastChild.OuterXml);
		}
	}
}

