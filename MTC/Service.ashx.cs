using System.Web;

namespace MTC
{
    public class Service : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var agk = new Agk().GetQuotation();
            var spMundi = new SpMundi().GetQuotation();
            var json = $@"{{""Cotacao"":[{{""AGK"":[{agk}]}},{{""SPMUNDI"":[{spMundi}]}}]}}";

            context.Response.ContentType = "application/json";
            context.Response.Write(json);
        }

        public bool IsReusable => false;        
    }
}
