using System;
using Newtonsoft.Json;
namespace OmnitureApiConsumer.Models
{
    [JsonObject]
    public interface IOmnitureReport
    {
        [JsonProperty("reportID")]
        long Id { get; set; }
    }
}
