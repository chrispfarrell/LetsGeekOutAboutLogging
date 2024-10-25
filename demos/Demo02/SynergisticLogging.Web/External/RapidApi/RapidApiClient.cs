using System.Diagnostics;
using System.Net.Http.Headers;
using SynergisticLogging.Web.Logging;

namespace SynergisticLogging.Web.External.RapidApi
{
    public interface IRapidApiClient
    {
        Task<ApiResponse> Get(string url, Guid correlationId, string querystring = "");
    }
    public class RapidApiClient : IRapidApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly MyLogger _myLogger;

        public RapidApiClient(IConfiguration configuration, MyLogger myLogger)
        {
            _myLogger = myLogger;
            _configuration = configuration;
        }
        public async Task<ApiResponse> Get(string url, Guid correlationId, string querystring = "")
        {
            using var client = new HttpClient();

            var uriBuilder = new UriBuilder(url);

            if (!string.IsNullOrEmpty(querystring))
            {
                uriBuilder.Query = querystring;
            }

            // default is just JSON
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // add headers for Rapid API auth.
            client.DefaultRequestHeaders.Add("X-RapidAPI-Key", _configuration.GetValue<string>("X-RapidAPI-Key"));
            client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "dad-jokes.p.rapidapi.com");

            ApiResponse apiResponse = new()
            {
                Url = uriBuilder.Uri.ToString(),
                Verb = "GET",
                CorrelationId = correlationId
            };
            Exception exception = null;

            try
            {
                var sw = Stopwatch.StartNew();
                var responseMessage = await client.GetAsync(uriBuilder.Uri).ConfigureAwait(false);

                apiResponse.IsSuccessStatusCode = responseMessage.IsSuccessStatusCode;
                apiResponse.ElapsedMilliseconds = sw.ElapsedMilliseconds;
                apiResponse.StatusCode = responseMessage.StatusCode;
                apiResponse.ResponseBody = responseMessage.Content.ReadAsStringAsync().Result;
                
            }
            catch (Exception ex)
            {
                apiResponse.Exception = ex;
            }

            _myLogger.WriteApiHistory(apiResponse);

            return apiResponse;
        }
    }
}
