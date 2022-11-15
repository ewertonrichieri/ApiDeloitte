using System.Net;

namespace WebApiDeloitte.Model
{
    public class Response
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Body { get; set; }
        public string Error { get; set; }
    }
}
