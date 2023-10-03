using System.Net;

namespace DebtsCompass.Domain
{
    public class Response<T>
    {
        public string Message { get; set; }
        public T Payload { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
