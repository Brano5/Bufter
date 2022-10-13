using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography;
using System.Text;

namespace Bufter.Helpers
{
	public static class CustomHelper
	{
        public static string IsSelected(this IHtmlHelper htmlHelper, string controller, string action = "", string cssClass = "active")
        {
            string? currentAction = htmlHelper.ViewContext.RouteData.Values["action"] as string;
            string? currentController = htmlHelper.ViewContext.RouteData.Values["controller"] as string;

            if(action == "")
            {
                return controller.Equals(currentController) ? cssClass : String.Empty;
            }

            return action.Equals(currentAction) && controller.Equals(currentController) ? cssClass : String.Empty;
        }

        public static string IsSelected(this IHtmlHelper htmlHelper, string[] controllers, string[]? actions = null, string cssClass = "active")
        {
            string? currentAction = htmlHelper.ViewContext.RouteData.Values["action"] as string;
            string? currentController = htmlHelper.ViewContext.RouteData.Values["controller"] as string;

            if (actions == null)
            {
                return controllers.Contains(currentController) ? cssClass : String.Empty;
            }

            return actions.Contains(currentAction) && controllers.Contains(currentController) ? cssClass : String.Empty;
        }

        public static bool IsLogged(this IHtmlHelper htmlHelper)
        {
            return (htmlHelper.ViewContext.HttpContext.User.Identity != null && htmlHelper.ViewContext.HttpContext.User.Identity.Name != null);
        }

        public static string HashPassword(string password, string key)
        {
            // Convert the salted password to a byte array
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(password + key);
            // Use the hash algorithm to calculate the hash
            HashAlgorithm algorithm = new SHA256Managed();
            byte[] hash = algorithm.ComputeHash(saltedHashBytes);
            // Return the hash as a base64 encoded string to be compared to the stored password
            return Convert.ToBase64String(hash);
        }
    }
}
