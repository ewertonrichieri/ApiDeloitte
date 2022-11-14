using System.Net;

namespace WebApiDeloitte.Model.Response
{
    public class Response
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Msg { get; set; }
        public string Error { get; set; }
    }
}
