using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDelivaryBack
{
    public class AuthPermissions :Attribute, IAsyncActionFilter
    {
        private List<string> _permissions;
        public AuthPermissions(params string[] permissions)
        {
            _permissions = permissions.ToList();
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var permissions = context.HttpContext.User.Claims.Where(x => x.Type == ClaimConstants.Permissions)
                 .Select(x => x.Value).ToList();

            foreach (var permission in _permissions)
            {
                if (!permissions.Contains(permission))
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

                    var message = new ErrorMessage
                    {
                        Message = $"Action requires Permissions:{permissionToString()}"
                    };

                    await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(message));

                    return;
                }
            }

            await next.Invoke();
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
