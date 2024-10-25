namespace SynergisticLogging.Web.Logging
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class SensitiveInfoNoLogging : Attribute
    {

    }
}
