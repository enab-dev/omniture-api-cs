using System;
using Newtonsoft.Json;
namespace OmnitureApiConsumer.Models
{
    [JsonObject]
    public interface IOmnitureApiError
    {
        [JsonProperty("error")]
        string Error { get; set; }
        [JsonProperty("error_description")]
        string ErrorDescription { get; set; }
        [JsonProperty("error_uri")]
        string ErrorUri { get; set; }
    }
}
