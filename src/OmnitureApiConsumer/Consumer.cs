using System;
using System.IO;
using System.Net;
using System.Text;
using OmnitureApiConsumer.Models;
using OmnitureApiConsumer.Utils;

namespace OmnitureApiConsumer
{
    public class Consumer : IConsumer
    {
        /// <summary>
        /// Constructor takes three arguments in order to be able to make calls
        /// to Omniture API: username, shared secret key and the endpoint
        /// </summary>
        /// <param name="username">Omniture username</param>
        /// <param name="secret">Shared secret key</param>
        /// <param name="endpoint">Service endpoint</param>
        public Consumer(string username, string secret, string endpoint)
        {
            Username = username;
            Secret = secret;
            Endpoint = endpoint;
        }

        /// <summary>
        /// Calls the Omniture API with the provided method and the post data. 
        /// The response is then returned as a string. Omniture API data is in
        /// JSON format so the JSON should be parsed as needed by the caller.
        /// </summary>
        /// <param name="method">Omniture API method to call</param>
        /// <param name="postData">Data to post to Omniture</param>
        /// <returns></returns>
        public IOmnitureApiResponse CallOmnitureApi(string method, string postData)
        {
            if(string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Secret) || string.IsNullOrEmpty(Endpoint))
            {
                throw new ArgumentException("Username, Secret and Endpoing must all be set before this method can be called.");
            }

            var request = SetUpApiRequest(method, postData);
            WebResponse response;

            try
            {
                response = request.GetResponse();
            }
            catch (WebException ex)
            {
                response = ex.Response;
            }

            return GetApiResponse(response);
        }

        #region Authentication Properties

        public string Username { get; set; }
        public string Secret { get; set; }
        public string Endpoint { get; set; } 
        
        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Gets the Omniture API response from the web response object.
        /// </summary>
        /// <param name="response">Web response to API request</param>
        /// <returns>Omniture API response</returns>
        private IOmnitureApiResponse GetApiResponse(WebResponse response)
        {
            var httpResponse = (HttpWebResponse)response;

            using (var data = response.GetResponseStream())
            using (var reader = new StreamReader(data))
            {
                return new OmnitureApiResponse { StatusCode = httpResponse.StatusCode, ResponseData = reader.ReadToEnd() };
            }
        }

        /// <summary>
        /// Sets up the Omniture API request. Creates an HTTP1.1 POST request from the method provided
        /// and adds authentication headers and post data to it.
        /// </summary>
        /// <param name="method">Omniture API method to call</param>
        /// <param name="data">POST data to add to the request</param>
        /// <returns>Omniture API request object</returns>
        private HttpWebRequest SetUpApiRequest(string method, string data)
        {
            var postData = EncodePostData(data);

            var url = new StringBuilder(string.Format("{0}/admin/1.4/rest/?method={1}", Endpoint, method));

            var omnitureRequest = (HttpWebRequest) WebRequest.Create(url.ToString());

            omnitureRequest.ProtocolVersion = HttpVersion.Version11;
            omnitureRequest.Method = "POST";

            omnitureRequest.Headers.Add(OmnitureAuthUtils.GetAuthenticationHeaders(Username, Secret));

            omnitureRequest.ContentType = "application/x-www-form-urlencoded";
            omnitureRequest.ContentLength = postData.Length;
            AddPostDataToRequest(omnitureRequest, postData);

            return omnitureRequest;
        }

        /// <summary>
        /// Encodes the post data into a byte array.
        /// </summary>
        /// <param name="postData">Data string to encode</param>
        /// <returns>Encoded byte array</returns>
        private byte[] EncodePostData(string postData)
        {
            var encoding = new UTF8Encoding();
            return encoding.GetBytes(postData);
        }

        /// <summary>
        /// Takes the provided post data byte array and adds it to the request stream.
        /// </summary>
        /// <param name="request">Request to which to add post data</param>
        /// <param name="postData">Post data to add to the request</param>
        private static void AddPostDataToRequest(HttpWebRequest request, byte[] postData)
        {
            var stream = request.GetRequestStream();
            stream.Write(postData, 0, postData.Length);
            stream.Close();
        }

        #endregion

    }
}
