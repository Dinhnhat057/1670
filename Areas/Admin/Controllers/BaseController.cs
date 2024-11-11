using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;
using TuyenDungCore.Models.Dtos;

namespace TuyenDungCore.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        protected UserLogin? UserLogin()
        {
            var accountString = HttpContext.Session.GetString(Commons.CommonConstants.ADMIN_SESSION);
            if (string.IsNullOrEmpty(accountString)) return null;
            UserLogin? userLogin = JsonSerializer.Deserialize<UserLogin>(accountString);
            return userLogin;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var accountString = HttpContext.Session.GetString(Commons.CommonConstants.ADMIN_SESSION);
            if (string.IsNullOrEmpty(accountString))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login" }));
            }
            else
            {
                UserLogin? userLogin = JsonSerializer.Deserialize<UserLogin>(accountString);
                if (userLogin != null)
                {

                }
                else
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login" }));
                }
            }
            base.OnActionExecuting(context);
        }

        protected void SetAlert(string message, string type)
        {
            TempData["Notify"] = message;
            if (type == "success")
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (type == "error")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }
    }
}
