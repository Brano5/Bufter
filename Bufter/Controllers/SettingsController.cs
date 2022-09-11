using Bufter.Data;
using Bufter.Model;
using Bufter.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Bufter.Controllers
{
	public class SettingsController : Controller
	{
        private readonly ApplicationDBContext _db;

        public SettingsController(ApplicationDBContext db)
        {
            _db = db;
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
            settings.Bill = _db.Persons.Where(a => a.Name == settings.Person).FirstOrDefault().Bill;
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
            return View("Settings", settings);
        }

        public IActionResult AddMoney(string person, string amount)
        {
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
                personDb.Bill +=  Convert.ToDouble(amount.Replace(".", ","));
            }
            _db.Persons.Update(personDb);
            _db.SaveChanges();

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
