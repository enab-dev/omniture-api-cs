using System.Net;

namespace OmnitureApiConsumer.Models
{
    public class OmnitureApiResponse : IOmnitureApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ResponseData { get; set; }
    }
}
