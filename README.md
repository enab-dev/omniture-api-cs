omniture-api-cs
===============

Adobe Marketing Cloud (Omniture) API v1.4 C# Library

This library will allow you to access the Adobe Marketing Cloud Analytics API 1.4 as a REST client. All responses are in JSON format. This library abstracts the authentication and API calls thus reducing the effort to access the API to something like this:

```
var apiResponse = new Consumer(Username, Secret, Endpoint).CallOmnitureApi(method, postData);

if (IsResponseValid(apiResponse))
{
    // Do whatever you want with the response
}
else
{
    var error = apiResponse.ResponseData.GetOmnitureErrors();

    if (error != null && !string.IsNullOrEmpty(error.Error) && !error.Error.Equals("report_not_ready"))
    {
    	// This is a real error and should be handled
        Logger.ErrorFormat("Error: {0}\nError Description: {1}", error.Error, error.ErrorDescription);
    }

	// The report is simply not ready. Print out the message for the user and retry after some time
    OmnitureReport report = JsonConvert.DeserializeObject<OmnitureReport>(postData);
    Logger.WarnFormat("Report not ready! Retrying to get report {0} in {1} seconds.", report.Id, RetryInSeconds);

    // Wait and retry logic goes here
```

Planned enhancements include unit tests and classes to access reports via the API. 