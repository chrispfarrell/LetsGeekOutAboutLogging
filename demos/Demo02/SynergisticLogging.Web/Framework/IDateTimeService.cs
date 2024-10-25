namespace SynergisticLogging.Web.Framework;

public interface IDateTimeService
{
    DateTime Now();
}

public class DateTimeService : IDateTimeService
{
    public DateTime Now()
    {
        return DateTime.Now;
    }
}