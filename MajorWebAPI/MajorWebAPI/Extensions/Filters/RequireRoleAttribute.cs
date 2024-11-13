using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MajorWebAPI.Extensions.Filters
{
    public class RequireRoleAttribute :ActionFilterAttribute
    {
        private readonly string _roleName;
        public RequireRoleAttribute(string roleName) 
        {
            _roleName = roleName;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;

            // Check if the user is authenticated and has the required role
            if (!user.Identity.IsAuthenticated || !user.IsInRole(_roleName))
            {
                // Return 403 Forbidden if the check fails
                context.Result = new ForbidResult();
            }
        }
    }
}
