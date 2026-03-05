using Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Presentation.ActionFilters
{
    public class ProductCreateValidationFilter : IActionFilter
    {
        //this runs after the action on which the filter is applied completes
        public void OnActionExecuted(ActionExecutedContext context)
        {
             
        }

        //this runs before the action on which the filter is applied starts to execute
        public void OnActionExecuting(ActionExecutingContext context)
        {
           if(context.HttpContext.User.Identity.IsAuthenticated == false)
            {
                context.Result = new ForbidResult();
            }
            else
            {
                var userManager =  context.HttpContext.RequestServices.GetService<UserManager<CustomUser>>();
                if (userManager != null)
                {
                    Task<CustomUser> t = userManager.GetUserAsync(context.HttpContext.User);
                    //here i can do other things
                    t.Wait();
                    var myUser = t.Result;

                    if(myUser.CreateProductCapability)
                    {
                    
                    }
                    else
                    {
                        context.Result = new ForbidResult();
                    }
                }
            }
        }
    }
}
