using Newtonsoft.Json;

namespace SynergisticLogging.Web.Logging.ActionLogger
{
    public class ActionLog
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }

        #region Data fields from before controller code is called in OnActionExecuting
        public string Verb { get; set; } = string.Empty;
        public string Controller { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string ModelArguments { get; set; } = string.Empty;
        public string RemoteIpAddress { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        #endregion

        #region Data fields available ony after the code has executed
        public int ElapsedMilliseconds { get; set; } = 0;
        public Exception? Exception { get; set; } = null;
        #endregion

        public override string ToString()
        {
            return JsonConvert.SerializeObject(ModelArguments);
        }
    }
}
