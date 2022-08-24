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
            if (checkCookies())
                return RedirectToAction("Index", "Home");
            if (Request.Cookies["SaveRoom"] == "True")
            {
                if (Request.Cookies["Room"] != null && Request.Cookies["Room"] != "")
                {
                    if (Request.Cookies["SavePerson"] == "True")
                    {
                        if (Request.Cookies["Person"] != null && Request.Cookies["Person"] != "")
                        {
                            int roomId1 = _db.Rooms.Where(a => a.Name == Request.Cookies["Room"]).FirstOrDefault().Id;
                            return View("Item", new Tuple<IEnumerable<Room>, IEnumerable<Item>>(_db.Rooms, _db.Items.Where(a => a.RoomId == roomId1 || a.RoomId == -1)));
                        }
                    }
                    int roomId2 = _db.Rooms.Where(a => a.Name == Request.Cookies["Room"]).FirstOrDefault().Id;
                    return View("Person", new Tuple<IEnumerable<Person>, IEnumerable<Person>>(_db.Persons, _db.Persons.Where(a => a.RoomId == roomId2 || a.RoomId == -1)));
                }
            }
            return Room();
        }

        public IActionResult Room()
        {
            return View("Room", _db.Rooms);
        }

        public IActionResult Person(string room)
        {
            int roomId = _db.Rooms.Where(a => a.Name == room).FirstOrDefault().Id;
            Response.Cookies.Append("Room", room);

            if (Request.Cookies["SavePerson"] == "True")
            {
                if (Request.Cookies["Person"] != null && Request.Cookies["Person"] != "")
                {
                    return View("Item", new Tuple<IEnumerable<Room>, IEnumerable<Item>>(_db.Rooms, _db.Items.Where(a => a.RoomId == roomId || a.RoomId == -1)));
                }
            }

            return View("Person", new Tuple<IEnumerable<Room>, IEnumerable<Person>>(_db.Rooms, _db.Persons.Where(a => a.RoomId == roomId || a.RoomId == -1)));
        }

        public IActionResult Item(string room, string person)
        {
            int roomId = _db.Rooms.Where(a => a.Name == room).FirstOrDefault().Id;
            Response.Cookies.Append("Room", room);
            Response.Cookies.Append("Person", person);

            return View("Item", new Tuple<IEnumerable<Room>, IEnumerable<Item>>(_db.Rooms, _db.Items.Where(a => a.RoomId == roomId || a.RoomId == -1)));
        }

        public IActionResult Order(string room, string person, string item)
        {
            Item itemDb = _db.Items.Where(a => a.Name == item).FirstOrDefault();
            itemDb.Count--;
            double price = itemDb.Price;
            _db.Items.Update(itemDb);
            _db.SaveChanges();

            Person personDb = _db.Persons.Where(a => a.Name == person).FirstOrDefault();
            personDb.TotalBill += price;
            personDb.Bill -= price;
            _db.Persons.Update(personDb);
            _db.SaveChanges();

            return Item(room, person);
            //return View("Item", new Tuple<IEnumerable<Item>, string, string>(_db.Items, room, person));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private bool checkCookies()
        {
            bool change = false;
            if (Request.Cookies["SaveRoom"] == null)
            {
                Response.Cookies.Append("SaveRoom", "True");
                change = true;
            }
            if (Request.Cookies["SavePerson"] == null)
            {
                Response.Cookies.Append("SavePerson", "True");
                change = true;
            }
            return change;
        }
    }
}