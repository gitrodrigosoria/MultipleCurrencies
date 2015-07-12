using System;
using System.Web;

namespace MTC
{
    /// <summary>
    /// Summary description for mtc
    /// </summary>
    public class mtc : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(@"{""cotacao"":[");
            context.Response.Write(@"{""AGK"":[" + new Agk().getQuotation() + "]},");
            context.Response.Write(@"{""SPMUNDI"":[" + new SpMundi().getQuotation() + "]}");
            context.Response.Write(@"]}");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
