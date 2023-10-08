using System.Net;

namespace DebtsCompass.Domain
{
    public class Response<T>
    {
        public string Message { get; set; }
        public T Payload { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public Response(string message, T payload, HttpStatusCode statusCode)
        {
            Message = message;
            Payload = payload;
            StatusCode = statusCode;
        }

        public Response() { }
    }
}
