using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ASB.Integration.Assessment.WebAPI.DatabaseContext.EntityModels;

namespace ASB.Integration.Assessment.WebAPI.Common
{
    /// <summary>
    /// Authorize attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        /// <inheritdoc/>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (UserLoginCredentialEntity)context.HttpContext.Items["User"];
            if (user == null)
            {
                // not logged in
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
