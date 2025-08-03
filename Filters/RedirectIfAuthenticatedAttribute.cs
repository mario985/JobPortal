using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class RedirectIfAuthenticatedAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var user = context.HttpContext.User;
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId != null && user.Identity.IsAuthenticated)
        {

            {
                context.Result = new RedirectToActionResult("Index", "Job", null);
            }
        }
        base.OnActionExecuting(context);
        
    }
}