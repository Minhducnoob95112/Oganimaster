using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Oganimaster.Models;
using System.Security.Cryptography.Pkcs;
namespace Oganimaster.Models.Authentication
{
    public class Authentication : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
           if(context.HttpContext.Session.GetString("UserName") == null)
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        { "Controller", "Access" },
                        { "Action", "Login" }
                    });
            }
           base.OnActionExecuting(context);
        }
    }
}
