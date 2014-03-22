using System.Net;
namespace OmnitureApiConsumer.Models
{
    public interface IOmnitureApiResponse
    {
        string ResponseData { get; set; }
        HttpStatusCode StatusCode { get; set; }
    }
}
