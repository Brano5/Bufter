using Bufter.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bufter.Controllers
{
	public class SettingsController : Controller
	{
		public IActionResult Index()
		{
			Settings settings = new Settings();
			settings.SaveRoom = Request.Cookies["SaveRoom"] == "True" ? true : false;
            settings.SavePerson = Request.Cookies["SavePerson"] == "True" ? true : false;
            settings.Room = Request.Cookies["Room"];
            settings.Person = Request.Cookies["Person"];
            return View("Settings", settings);
		}

        [HttpPost]
        public IActionResult Save(Settings settings)
        {
            Response.Cookies.Append("SaveRoom", settings.SaveRoom.ToString());
            Response.Cookies.Append("SavePerson", settings.SavePerson.ToString());
            if(settings.Room != null)
            {
                Response.Cookies.Append("Room", settings.Room);
            }
            else
            {
                Response.Cookies.Delete("Room");
            }
            if(settings.Person != null)
            {
                Response.Cookies.Append("Person", settings.Person);
            }
            else
            {
                Response.Cookies.Delete("Person");
            }
            return View("Settings", settings);
        }
    }
}
