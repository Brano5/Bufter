using Bufter.Data;
using Bufter.Model;
using Bufter.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO.Pipelines;

namespace Bufter.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDBContext _db;
        private readonly LogManager _logManager;

        public HomeController(ApplicationDBContext db, LogManager logManager)
        {
            _db = db;
            _logManager = logManager;
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
                            Room? room = _db.Rooms.Where(a => a.Name == Request.Cookies["Room"]).FirstOrDefault();
                            if(room != null)
                            {
                                var find = _db.Persons.Where(a => a.Name == Request.Cookies["Person"]).FirstOrDefault();
                                if (find != null)
                                {
                                    double bill = find.Bill;
                                    return View("Item", new Tuple<IEnumerable<Room>, IEnumerable<Item>, double>(_db.Rooms, _db.Items.Where(a => a.RoomId == room.Id || a.RoomId == -1), bill));
                                }
                            }
                        }
                    }
                    Room? room2 = _db.Rooms.Where(a => a.Name == Request.Cookies["Room"]).FirstOrDefault();
                    if (room2 != null)
                    {
                        return View("Person", new Tuple<IEnumerable<Room>, IEnumerable<Person>>(_db.Rooms, _db.Persons.Where(a => a.RoomId == room2.Id || a.RoomId == -1)));
                    }
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
            if(room == null)
            {
                return Room();
            }
            var find = _db.Rooms.Where(a => a.Name == room).FirstOrDefault();
            if (find == null)
            {
                return Room();
            }
            int roomId = find.Id;
            Response.Cookies.Append("Room", room, new CookieOptions { Expires = DateTime.Now.AddYears(10) });

            if (Request.Cookies["SavePerson"] == "True" && Request.Cookies["SaveRoom"] == "False")
            {
                if (Request.Cookies["Person"] != null && Request.Cookies["Person"] != "")
                {
                    var find2 = _db.Persons.Where(a => a.Name == Request.Cookies["Person"]).FirstOrDefault();
                    if (find2 != null)
                    {
                        double bill = find2.Bill;
                        return View("Item", new Tuple<IEnumerable<Room>, IEnumerable<Item>, double>(_db.Rooms, _db.Items.Where(a => a.RoomId == roomId || a.RoomId == -1), bill));
                    }
                }
            }

            return View("Person", new Tuple<IEnumerable<Room>, IEnumerable<Person>>(_db.Rooms, _db.Persons.Where(a => a.RoomId == roomId || a.RoomId == -1)));
        }

        public IActionResult PersonHard(string room)
        {
            if (room == null)
            {
                return Room();
            }
            var find = _db.Rooms.Where(a => a.Name == room).FirstOrDefault();
            if (find == null)
            {
                return Room();
            }
            int roomId = find.Id;
            Response.Cookies.Append("Room", room, new CookieOptions { Expires = DateTime.Now.AddYears(10) });

            return View("Person", new Tuple<IEnumerable<Room>, IEnumerable<Person>>(_db.Rooms, _db.Persons.Where(a => a.RoomId == roomId || a.RoomId == -1)));
        }

        public IActionResult Item(string room, string person)
        {
            if (room == null)
            {
                return Room();
            }
            if (person == null)
            {
                return Person(room);
            }
            var find = _db.Rooms.Where(a => a.Name == room).FirstOrDefault();
            if (find == null)
            {
                return Room();
            }
            int roomId = find.Id;
            Response.Cookies.Append("Room", room, new CookieOptions { Expires = DateTime.Now.AddYears(10)});
            Response.Cookies.Append("Person", person, new CookieOptions { Expires = DateTime.Now.AddYears(10) });
            var find2 = _db.Persons.Where(a => a.Name == person).FirstOrDefault();
            if (find2 == null)
            {
                return Person(room);
            }
            double bill = find2.Bill;

            return View("Item", new Tuple<IEnumerable<Room>, IEnumerable<Item>, double>(_db.Rooms, _db.Items.Where(a => a.RoomId == roomId || a.RoomId == -1), bill));
        }

        public IActionResult Order(string room, string person, string item)
        {
            if (room == null)
            {
                return Room();
            }
            if (person == null)
            {
                return Person(room);
            }
            if (item == null)
            {
                return Item(room, person);
            }
            Item? itemDb = _db.Items.Where(a => a.Name == item).FirstOrDefault();
            if (itemDb == null)
            {
                return Item(room, person);
            }
            if (itemDb.Count != 0)
            {
                itemDb.Count--;
            }
            double price = itemDb.Price;
            _db.Items.Update(itemDb);
            _db.SaveChanges();

            Person? personDb = _db.Persons.Where(a => a.Name == person).FirstOrDefault();
            if (personDb == null)
            {
                return Person(room);
            }
            personDb.TotalBill += price;
            personDb.Bill -= price;
            _db.Persons.Update(personDb);
            _db.SaveChanges();

            _logManager.addBuyLog(room, person, item, "Buy item", HttpContext);
            @TempData["Info"] = "Succesfull Buy item!";

            //return Person(room);
            return Index();
            //return Item(room, person);
            //return View("Item", new Tuple<IEnumerable<Item>, string, string>(_db.Items, room, person));
        }

        public IActionResult AddMoney(string room, string person, string amount)
        {
            Person? personDb = _db.Persons.Where(a => a.Name == person).FirstOrDefault();
            if (personDb == null)
            {
                return Item(room, person);
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

            return Item(room, person);
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