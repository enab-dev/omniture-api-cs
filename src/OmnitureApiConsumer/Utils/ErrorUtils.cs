using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using OmnitureApiConsumer.Models;

namespace OmnitureApiConsumer.Utils
{
    public static class ErrorExtension
    {
        // JSON schema used to validate the error JSON string
        private static readonly JsonSchema ErrorSchema = JsonSchema.Parse(@"{
            'description': 'Omniture Analytics Report Errors',
            'type': 'object',
            'properties':
            {
                'error': {'type':'string'},
                'error_description': {'type':'string'}
            }
        }");
        
        /// <summary>
        /// String extension method to deserialize Omniture API error messages
        /// The list of possible error messages can be found here:
        /// https://marketing.adobe.com/developer/en_US/documentation/analytics-reporting-1-4/errors
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IOmnitureApiError GetOmnitureErrors(this string data)
        {
            var json = JObject.Parse(data);
            return json.IsValid(ErrorSchema) ? JsonConvert.DeserializeObject<OmnitureApiError>(data) : null;
        }
    }
}
