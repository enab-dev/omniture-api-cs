using System;
using OmnitureApiConsumer.Models;
namespace OmnitureApiConsumer
{
    public interface IConsumer
    {
        IOmnitureApiResponse CallOmnitureApi(string method, string postData);
        string Endpoint { get; set; }
        string Secret { get; set; }
        string Username { get; set; }
    }
}
