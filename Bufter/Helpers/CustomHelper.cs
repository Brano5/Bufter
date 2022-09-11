using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bufter.Helpers
{
	public static class CustomHelper
	{
        public static string IsSelected(this IHtmlHelper htmlHelper, string controllers, string actions = "", string cssClass = "active")
        {
            string? currentAction = htmlHelper.ViewContext.RouteData.Values["action"] as string;
            string? currentController = htmlHelper.ViewContext.RouteData.Values["controller"] as string;

            if(actions == "")
            {
                return controllers.Contains(currentController) ? cssClass : String.Empty;
            }

            return actions.Contains(currentAction) && controllers.Contains(currentController) ? cssClass : String.Empty;
        }
    }
}
