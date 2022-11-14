using Bufter.Data;
using Bufter.Model;
using Bufter.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Xml.Linq;
using System.Globalization;

namespace Bufter.Controllers
{
	public class ManageController : Controller
	{
        private readonly ApplicationDBContext _db;
        private readonly IWebHostEnvironment env;
        private readonly LogManager _logManager;

        public ManageController(ApplicationDBContext db, IWebHostEnvironment environment, LogManager logManager)
		{
			_db = db;
            env = environment;
            _logManager = logManager;
        }

		public ActionResult Index()
		{
			return View("ManageRoom", _db.Rooms);
		}

        public IActionResult ManageRoom()
        {
            return View("ManageRoom", _db.Rooms);
        }

        public IActionResult ManagePerson()
        {
            return View("ManagePerson", new Tuple<IEnumerable<Room>, IEnumerable<Person>>(_db.Rooms, _db.Persons));
        }

        [HttpGet]
        public IActionResult ManagePersonSearch(int RoomId, string Search)
        {
            IEnumerable<Person>? persons = null;
            if(RoomId == -1 && Search == null)
                return ManagePerson();
            if(RoomId == -1)
                persons = _db.Persons.Where(a => a.Name.Contains(Search));
            if (Search == null)
                persons = _db.Persons.Where(a => a.RoomId == RoomId || a.RoomId == -1);
            if (RoomId != -1 && Search != null)
                persons = _db.Persons.Where(a => (a.RoomId == RoomId || a.RoomId == -1) && a.Name.Contains(Search));
            return View("ManagePerson", new Tuple<IEnumerable<Room>, IEnumerable<Person>>(_db.Rooms, persons));
        }

        public IActionResult ManageItem()
        {
            return View("ManageItem", new Tuple<IEnumerable<Room>, IEnumerable<Item>>(_db.Rooms, _db.Items));
        }

        [HttpGet]
        public IActionResult ManageItemSearch(int RoomId, string Search)
        {
            IEnumerable<Item>? items = null;
            if (RoomId == -1 && Search == null)
                return ManageItem();
            if (RoomId == -1)
                items = _db.Items.Where(a => a.Name.Contains(Search));
            if (Search == null)
                items = _db.Items.Where(a => a.RoomId == RoomId || a.RoomId == -1);
            if (RoomId != -1 && Search != null)
                items = _db.Items.Where(a => (a.RoomId == RoomId || a.RoomId == -1) && a.Name.Contains(Search));
            return View("ManageItem", new Tuple<IEnumerable<Room>, IEnumerable<Item>>(_db.Rooms, items));
        }

        [HttpPost]
        public IActionResult CreateRoom(string Name, string Description, IFormFile Image)
        {
            if (Name == null || Name == "" || _db.Rooms.Where(a => a.Name == Name).Count() != 0)
            {
                @TempData["Warning"] = "Wrong name!";
                
                return ManageRoom();
            }
            if (Description == null)
                Description = "";
                
            Room room = new Room();
            room.Name = Name;
            room.Description = Description;
            if (Image != null)
            {
                var uniqueFileName = GetUniqueFileName(Image.FileName);
                var uploads = Path.Combine(env.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, uniqueFileName);
                Image.CopyTo(new FileStream(filePath, FileMode.Create));
                room.Image = uniqueFileName;
            }
            else
            {
                room.Image = "";
            }
            room.Created = DateTime.Now;
            room.Updated = DateTime.Now;
            _db.Rooms.Add(room);
            _db.SaveChanges();

            _logManager.addLog("INFO", "Created Room " + Name + " by " + Request.Cookies["Person"], HttpContext);
            @TempData["Info"] = "Successfully created!";
            
            return ManageRoom();
        }

        [HttpPost]
        public IActionResult EditRoom(int Id, string Name, string Description, IFormFile Image)
        {
            if (Name == null || Name == "" || _db.Rooms.Where(a => a.Name == Name).Count() > 1)
            {
                @TempData["Warning"] = "Wrong name!";

                return ManageRoom();
            }
            if (Description == null)
                Description = "";

            Room? room = _db.Rooms.Find(Id);
            if(room == null)
            {
                return ManageRoom();
            }
            room.Name = Name;
            room.Description = Description;
            if (Image != null)
            {
                var uniqueFileName = GetUniqueFileName(Image.FileName);
                var uploads = Path.Combine(env.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, uniqueFileName);
                Image.CopyTo(new FileStream(filePath, FileMode.Create));
                room.Image = uniqueFileName;
            }
            room.Updated = DateTime.Now;
            _db.Rooms.Update(room);
            _db.SaveChanges();

            _logManager.addLog("INFO", "Edited Room " + Name + " by " + Request.Cookies["Person"], HttpContext);
            @TempData["Info"] = "Successfully edited!";
            
            return ManageRoom();
        }

        [HttpPost]
        public IActionResult DeleteRoom(int Id)
        {
            _db.Rooms.Remove(_db.Rooms.Find(Id));
            _db.SaveChanges();

            _logManager.addLog("INFO", "Deleted Room " + Id + " by " + Request.Cookies["Person"], HttpContext);
            @TempData["Info"] = "Successfully deleted!";
            
            return ManageRoom();
        }

        [HttpPost]
        public IActionResult CreatePerson(string Name, int RoomId, IFormFile Image)
        {
            if (Name == null || Name == "" || _db.Persons.Where(a => a.Name == Name).Count() != 0)
            {
                @TempData["Warning"] = "Wrong name!";

                return ManagePerson();
            }

            Person person = new Person();
            person.Name = Name;
            person.RoomId = RoomId;
            if (Image != null)
            {
                var uniqueFileName = GetUniqueFileName(Image.FileName);
                var uploads = Path.Combine(env.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, uniqueFileName);
                Image.CopyTo(new FileStream(filePath, FileMode.Create));
                person.Image = uniqueFileName;
            }
            else
            {
                person.Image = "";
            }
            person.Bill = 0;
            person.TotalBill = 0;
            person.Created = DateTime.Now;
            person.Updated = DateTime.Now;
            _db.Persons.Add(person);
            _db.SaveChanges();

            _logManager.addLog("INFO", "Created Person " + Name + " by " + Request.Cookies["Person"], HttpContext);
            @TempData["Info"] = "Successfully created!";
            
            return ManagePerson();
        }

        [HttpPost]
        public IActionResult EditPerson(int Id, string Name, int RoomId, string Bill, string TotalBill, IFormFile Image)
        {
            if (Name == null || Name == "" || _db.Persons.Where(a => a.Name == Name).Count() > 1)
            {
                @TempData["Warning"] = "Wrong name!";

                return ManagePerson();
            }
            if (Bill == null)
                Bill = "0";
            if (TotalBill == null)
                TotalBill = "0";

            Person? person = _db.Persons.Find(Id);
            if(person == null)
            {
                return ManagePerson();
            }
            person.Name = Name;
            person.RoomId = RoomId;
            if (Image != null)
            {
                var uniqueFileName = GetUniqueFileName(Image.FileName);
                var uploads = Path.Combine(env.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, uniqueFileName);
                Image.CopyTo(new FileStream(filePath, FileMode.Create));
                person.Image = uniqueFileName;
            }
            person.Bill = Math.Round(double.Parse(Bill, CultureInfo.InvariantCulture.NumberFormat), 2);
            person.TotalBill = Math.Round(double.Parse(TotalBill, CultureInfo.InvariantCulture.NumberFormat), 2);
            person.Updated = DateTime.Now;
            _db.Persons.Update(person);
            _db.SaveChanges();

            _logManager.addLog("INFO", "Edited Person " + Name + " by " + Request.Cookies["Person"], HttpContext);
            @TempData["Info"] = "Successfully edited!";
            
            return ManagePerson();
        }

        [HttpPost]
        public IActionResult DeletePerson(int Id)
        {
            _db.Persons.Remove(_db.Persons.Find(Id));
            _db.SaveChanges();

            _logManager.addLog("INFO", "Deleted Person " + Id + " by " + Request.Cookies["Person"], HttpContext);
            @TempData["Info"] = "Successfully deleted!";
            
            return ManagePerson();
        }

        [HttpPost]
        public IActionResult CreateItem(string Name, string Description, int RoomId, int Count, string Price, IFormFile Image)
        {
            if (Name == null || Name == "" || _db.Persons.Where(a => a.Name == Name).Count() != 0)
            {
                @TempData["Warning"] = "Wrong name!";

                return ManageItem();
            }
            if (Description == null)
                Description = "";
            if (Price == null)
                Price = "0";

            Item item = new Item();
            item.Name = Name;
            item.Description = Description;
            item.RoomId = RoomId;
            if (Image != null)
            {
                var uniqueFileName = GetUniqueFileName(Image.FileName);
                var uploads = Path.Combine(env.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, uniqueFileName);
                Image.CopyTo(new FileStream(filePath, FileMode.Create));
                item.Image = uniqueFileName;
            }
            else
            {
                item.Image = "";
            }
            item.Count = Count;
            item.Price = Math.Round(double.Parse(Price, CultureInfo.InvariantCulture.NumberFormat), 2);
            item.Created = DateTime.Now;
            item.Updated = DateTime.Now;
            _db.Items.Add(item);
            _db.SaveChanges();

            _logManager.addLog("INFO", "Created Item " + Name + " by " + Request.Cookies["Person"], HttpContext);
            @TempData["Info"] = "Successfully created!";
            
            return ManageItem();
        }

        [HttpPost]
        public IActionResult EditItem(int Id, string Name, string Description, int RoomId, int Count, string Price, IFormFile Image)
        {
            if (Name == null || Name == "" || _db.Persons.Where(a => a.Name == Name).Count() > 1)
            {
                @TempData["Warning"] = "Wrong name!";

                return ManageItem();
            }
            if (Description == null)
                Description = "";
            if (Price == null)
                Price = "0";

            Item? item = _db.Items.Find(Id);
            if (item == null)
            {
                return ManageItem();
            }
            item.Name = Name;
            item.Description = Description;
            item.RoomId = RoomId;
            if (Image != null)
            {
                var uniqueFileName = GetUniqueFileName(Image.FileName);
                var uploads = Path.Combine(env.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, uniqueFileName);
                Image.CopyTo(new FileStream(filePath, FileMode.Create));
                item.Image = uniqueFileName;
            }
            item.Count = Count;
            item.Price = Math.Round(double.Parse(Price, CultureInfo.InvariantCulture.NumberFormat), 2);
            item.Updated = DateTime.Now;
            _db.Items.Update(item);
            _db.SaveChanges();

            _logManager.addLog("INFO", "Edited Item " + Name + " by " + Request.Cookies["Person"], HttpContext);
            @TempData["Info"] = "Successfully edited!";
            
            return ManageItem();
        }

        [HttpPost]
        public IActionResult DeleteItem(int Id)
        {
            _db.Items.Remove(_db.Items.Find(Id));
            _db.SaveChanges();

            _logManager.addLog("INFO", "Deleted Item " + Id + " by " + Request.Cookies["Person"], HttpContext);
            @TempData["Info"] = "Successfully deleted!";
            
            return ManageItem();
        }

        public IActionResult AddMoney(string person, string amount)
        {
            Person? personDb = _db.Persons.Where(a => a.Name == person).FirstOrDefault();
            if (personDb == null)
            {
                return ManagePerson();
            }
            if(amount == "0")
            {
                personDb.Bill = 0;
            }
            else
            {
                personDb.Bill +=  Math.Round(double.Parse(amount, CultureInfo.InvariantCulture.NumberFormat), 2);
            }
            _db.Persons.Update(personDb);
            _db.SaveChanges();
            
            _logManager.addLog("INFO", "Add Money " + person + " amount " + amount + " by " + Request.Cookies["Person"], HttpContext);
            @TempData["Info"] = "Successfully added money!";

            return ManagePerson();
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }
    }
}
