namespace OmnitureApiConsumer.Models
{
    public class OmnitureApiError : IOmnitureApiError  
    {
        public string Error { get; set; }
        public string ErrorDescription { get; set; }
        public string ErrorUri { get; set; }
    }
}
