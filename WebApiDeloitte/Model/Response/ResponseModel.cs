using System;
using System.Net;

namespace WebApiDeloitte.Model.Response
{
    public class ResponseModel
    {
        public Response GetResponse(string msg, HttpStatusCode statusCode, string error)
        {
            try
            {
                return new Response() { Msg = msg, StatusCode = statusCode, Error = error };
            }
            catch (Exception ex)
            {
                return new Response() { StatusCode = HttpStatusCode.InternalServerError, Error = ex.Message };
            }
        }
    }
}
