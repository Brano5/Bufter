using Bufter.Data;
using Bufter.Model;
using Bufter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

namespace Bufter.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDBContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDBContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            if (Request.Cookies["SaveRoom"] == "True")
            {
                if (Request.Cookies["Room"] != null && Request.Cookies["Room"] != "")
                {
                    if (Request.Cookies["SavePerson"] == "True")
                    {
                        if (Request.Cookies["Person"] != null && Request.Cookies["Person"] != "")
                        {
                            return View("Item", new Tuple<IEnumerable<Item>, string, string>(_db.Items, Request.Cookies["Room"], Request.Cookies["Person"]));
                        }
                    }
                    return View("Person", new Tuple<IEnumerable<Person>, string>(_db.Persons, Request.Cookies["Room"]));
                }
            }
            return View("Room", _db.Rooms);
        }

        public IActionResult Room()
        {
            return View("Room", _db.Rooms);
        }

        public IActionResult Person(string room)
        {
            int roomId = _db.Rooms.Where(a => a.Name == room).FirstOrDefault().Id;
            if (Request.Cookies["SaveRoom"] == "True")
            {
                Response.Cookies.Append("Room", room);
            }
            
            return View("Person", new Tuple<IEnumerable<Room>, IEnumerable<Person>>(_db.Rooms, _db.Persons.Where(a => a.RoomId == roomId)));
        }

        public IActionResult Item(string room, string person)
        {
            int roomId = _db.Rooms.Where(a => a.Name == room).FirstOrDefault().Id;
            if (Request.Cookies["SaveRoom"] == "True")
            {
                Response.Cookies.Append("Room", room);
            }
            if (Request.Cookies["SavePerson"] == "True")
            {
                Response.Cookies.Append("Person", person);
            }
            return View("Item", new Tuple<IEnumerable<Room>, IEnumerable<Item>>(_db.Rooms, _db.Items.Where(a => a.RoomId == roomId)));
        }

        public IActionResult Order(string room, string person, string item)
        {
            return Item(room, person);
            //return View("Item", new Tuple<IEnumerable<Item>, string, string>(_db.Items, room, person));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}