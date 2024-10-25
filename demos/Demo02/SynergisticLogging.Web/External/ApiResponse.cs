using System.Net;

namespace SynergisticLogging.Web.External
{
    public class ApiResponse
    {
        public Guid CorrelationId { get; set; }
        public string Url { get; set; }
        public string Verb { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccessStatusCode { get; set; }
        public string ResponseBody { get; set; }
        public long ElapsedMilliseconds { get; set; }
        public Exception? Exception { get; set; }
    }
}
