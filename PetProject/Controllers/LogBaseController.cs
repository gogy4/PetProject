public abstract class LogBaseController : Controller
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        Console.WriteLine($"Controller: {context.Controller.GetType().Name}");
        Console.WriteLine($"Action: {context.ActionDescriptor.DisplayName}");

        base.OnActionExecuting(context);
    }
}