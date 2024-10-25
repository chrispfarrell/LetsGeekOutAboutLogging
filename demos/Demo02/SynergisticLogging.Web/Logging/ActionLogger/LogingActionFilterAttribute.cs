using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SynergisticLogging.Web.Logging.ActionLogger;

public class LoggingActionFilterAttribute : ActionFilterAttribute
{
    /// <summary>
    /// This event runs before the MVC or WebApi controller endpoint.  At this point in the lifecycle, the ModelBinder arguments are available to us.
    /// </summary>
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        base.OnActionExecuting(filterContext);

        // If logging is disabled on this controller, on this action, or this was not from a MVC/WebApi controller do not proceed
        if (IsLoggingDisabled(filterContext) || filterContext.Controller is not Controller controller)
        {
            return;
        }

        // Store current Tick count in ViewData
        controller.ViewData.Add("LoggingActionFilter_StartTicks", DateTime.Now.Ticks.ToString());

        // Get primitive value types (eg. Id) as passed into controller, and store as primitives.  Store models as JSON
        var actionParameters = filterContext.ActionArguments
            .Select(r => new
            {
                r.Key,
                Value = $"{(r.Value != null && (r.Value.GetType().IsPrimitive || r.Value is string || r.Value is decimal) ? r.Value : JsonConvert.SerializeObject(r.Value))}"
            });

        var actionDescriptor = filterContext.ActionDescriptor as ControllerActionDescriptor;
        var request = filterContext.HttpContext.Request;

        // Create an object that has the properties we want to log
        var actionLog = new ActionLog
        {
            Verb = request.Method,
            Controller = actionDescriptor?.ControllerName ?? "",
            Action = actionDescriptor?.ActionName ?? "",
            ModelArguments = JsonConvert.SerializeObject(actionParameters),
            RemoteIpAddress = filterContext.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "",
            UserName = request.HttpContext.User.Identity?.Name ?? ""
        };

        // Store the object in memory scoped to this request.  
        controller.ViewData.Add("LoggingActionFilter_ActionInfo", JsonConvert.SerializeObject(actionLog));
    }

    /// <summary>
    /// This event runs after the MVC or WebApi controller endpoint.  The ModelBinder data is no longer available but now we can get the outcomes.  We then pull data we stored
    /// in ViewData and further enrich it with ElapsedMilliseconds and any exceptions that occurred before Logging it.  
    /// </summary>
    public override void OnActionExecuted(ActionExecutedContext filterContext)
    {
        base.OnActionExecuted(filterContext);

        // If logging is disabled on this controller, on this action, or this was not from a MVC/WebApi controller do not proceed
        if (IsLoggingDisabled(filterContext) || filterContext.Controller is not Controller controller)
        {
            return;
        }

        var now = DateTime.Now.Ticks;

        var startTicks = long.Parse(controller.ViewData["LoggingActionFilter_StartTicks"]?.ToString() ?? "");
        var actionInfoString = controller.ViewData["LoggingActionFilter_ActionInfo"]?.ToString() ?? "";

        var actionInfo = JsonConvert.DeserializeObject<ActionLog>(actionInfoString);
        if (actionInfo == null) return;

        actionInfo.ElapsedMilliseconds = (int)(now - startTicks) / 10000; //10,000 ticks in a millisecond

        if (filterContext.Exception != null)
        {
            actionInfo.Exception = filterContext.Exception;
        }

        var logger = filterContext.HttpContext.RequestServices.GetRequiredService<MyLogger>();
        logger.LogAction(actionInfo);
    }

    /// <summary>
    /// Method to determine if the controller action had a no-logging attribute on it.
    /// </summary>
    private bool IsLoggingDisabled(FilterContext filterContext)
    {
        if (filterContext.ActionDescriptor is ControllerActionDescriptor actionDescriptor)
        {
            return actionDescriptor.MethodInfo.GetCustomAttributes<SensitiveInfoNoLogging>().Any();
        }

        return true;
    }
}