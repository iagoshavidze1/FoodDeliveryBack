using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodDelivaryBack
{
    public class AuthPermissions : Attribute, IActionFilter
    {
        private List<string> _permissions;
        public AuthPermissions(params string[] permissions)
        {

        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var permissions = context.HttpContext.User.Claims.Where(x => x.Type == ClaimConstants.Permissions)
                 .Select(x => x.Value).ToList();

            foreach (var permission in _permissions)
            {
                if (!permission.Contains(permission))
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

                    var message = new ErrorMessage
                    {
                        Message = $"Action requires Permissions:{permissionToString()}"
                    };

                    context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(message));
                }
            }
        }

        private string permissionToString()
        {
            var res = "";
            foreach (var permission in _permissions)
            {
                res += permission + ",";
            }

            return res.Substring(0, res.Length - 1);
        }
    }

    public class ErrorMessage
    {
        public string Message { get; set; }
    }
}
