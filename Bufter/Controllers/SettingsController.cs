using Bufter.Data;
using Bufter.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;

namespace Bufter.Controllers
{
	public class SettingsController : Controller
	{
        private readonly ApplicationDBContext _db;
        private readonly LogManager _logManager;

        public SettingsController(ApplicationDBContext db, LogManager logManager)
        {
            _db = db;
            _logManager = logManager;
        }

        public IActionResult Index()
		{
            if(checkCookies())
                return RedirectToAction("Index", "Settings");
            Settings settings = new Settings();
			settings.SaveRoom = Request.Cookies["SaveRoom"] == "True" ? true : false;
            settings.SavePerson = Request.Cookies["SavePerson"] == "True" ? true : false;
            settings.Room = Request.Cookies["Room"];
            settings.Person = Request.Cookies["Person"];
            var person = _db.Persons.Where(a => a.Name == settings.Person).FirstOrDefault();
            if(person != null)
            {
                settings.Bill = person.Bill;
            }
            return View("Settings", settings);
		}

        [HttpPost]
        public IActionResult Save(Settings settings)
        {
            Response.Cookies.Append("SaveRoom", settings.SaveRoom.ToString(), new CookieOptions { Expires = DateTime.Now.AddYears(10) });
            Response.Cookies.Append("SavePerson", settings.SavePerson.ToString(), new CookieOptions { Expires = DateTime.Now.AddYears(10) });
            if(settings.Room != null)
            {
                Response.Cookies.Append("Room", settings.Room, new CookieOptions { Expires = DateTime.Now.AddYears(10) });
            }
            else
            {
                Response.Cookies.Delete("Room");
            }
            if(settings.Person != null)
            {
                Response.Cookies.Append("Person", settings.Person, new CookieOptions { Expires = DateTime.Now.AddYears(10) });
            }
            else
            {
                Response.Cookies.Delete("Person");
            }

            _logManager.addLog("INFO", "Save settings", HttpContext);
            @TempData["Info"] = "Succesfull Save settings!";

            return View("Settings", settings);
        }

        public IActionResult AddMoney(string person, string amount)
        {
            if (person == null || person == "" || amount == null || amount == "")
            {
                @TempData["Waring"] = "Failed added money!";

                return Index();
            }
            Person? personDb = _db.Persons.Where(a => a.Name == person).FirstOrDefault();
            if(personDb == null)
            {
                return Index();
            }
            if (amount == "0")
            {
                personDb.Bill = 0;
            }
            else
            {
                personDb.Bill +=  Math.Round(double.Parse(amount, CultureInfo.InvariantCulture.NumberFormat), 2);
            }
            _db.Persons.Update(personDb);
            _db.SaveChanges();

            _logManager.addLog("INFO", "Add money", HttpContext);
            @TempData["Info"] = "Succesfull Added money!";

            return Index();
        }

        private bool checkCookies()
        {
            bool change = false;
            if (Request.Cookies["SaveRoom"] == null)
            {
                Response.Cookies.Append("SaveRoom", "True", new CookieOptions { Expires = DateTime.Now.AddYears(10) });
                change = true;
            }
            if (Request.Cookies["SavePerson"] == null)
            {
                Response.Cookies.Append("SavePerson", "True", new CookieOptions { Expires = DateTime.Now.AddYears(10) });
                change = true;
            }
            return change;
        }
    }
}
