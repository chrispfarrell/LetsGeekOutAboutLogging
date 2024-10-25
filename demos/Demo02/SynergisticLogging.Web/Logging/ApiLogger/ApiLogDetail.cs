
namespace SynergisticLogging.Web.Logging.ApiLogger;

public class ApiLogDetail
{
    public ApiLogDetail(Guid correlationId, int statusCode, DateTime timestamp)
    {
        CorrelationId = correlationId;
        StatusCode = statusCode;
        Timestamp = timestamp;  
    }
    public DateTime Timestamp { get; set; }
    public string Url { get; set; } = string.Empty;
    public string Verb { get; set; } = string.Empty;
    public int StatusCode { get; set; } 
    public string ResponseBody { get; set; } = string.Empty;
    public Guid CorrelationId { get; set; }
    public long ElapsedMilliseconds { get; set; } = 0;
    public Exception? Exception { get; set; } = null;
}