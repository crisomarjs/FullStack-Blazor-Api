using System.Net;

namespace ApiBlog.Repository
{
    public class RespuestasAPI
    {
        public RespuestasAPI()
        {
            ErrorMessage = new List<string>();
        }

        public HttpStatusCode statusCode { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> ErrorMessage { get; set; }
        public object Result { get; set; }
    }
}
