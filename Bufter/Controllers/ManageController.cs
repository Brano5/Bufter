using Bufter.Data;
using Bufter.Model;
using Bufter.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

namespace Bufter.Controllers
{
	public class ManageController : Controller
	{
        private readonly ApplicationDBContext _db;
        private readonly IWebHostEnvironment env;
        private readonly AlertManager aM;


        public ManageController(ApplicationDBContext db, IWebHostEnvironment environment, AlertManager alertManager)
		{
			_db = db;
            env = environment;
            aM = alertManager;
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
                //aM.addAlert("warning", "Wrong room name!");
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
            //aM.addAlert("success", "Room created successfully!");
            return ManageRoom();
        }

        [HttpPost]
        public IActionResult EditRoom(int Id, string Name, string Description, IFormFile Image)
        {
            if (Name == null || Name == "" || _db.Rooms.Where(a => a.Name == Name).Count() > 1)
            {
                //aM.addAlert("warning", "Wrong room name!");
                return ManageRoom();
            }
            if (Description == null)
                Description = "";

            Room room = _db.Rooms.Find(Id);
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
            //aM.addAlert("success", "Room updated successfully!");
            return ManageRoom();
        }

        [HttpPost]
        public IActionResult DeleteRoom(int Id)
        {
            _db.Rooms.Remove(_db.Rooms.Find(Id));
            _db.SaveChanges();
            //aM.addAlert("success", "Room created successfully!");
            return ManageRoom();
        }

        [HttpPost]
        public IActionResult CreatePerson(string Name, int RoomId, IFormFile Image)
        {
            if (Name == null || Name == "" || _db.Persons.Where(a => a.Name == Name).Count() != 0)
            {
                //aM.addAlert("warning", "Wrong room name!");
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
            //aM.addAlert("success", "Room created successfully!");
            return ManagePerson();
        }

        [HttpPost]
        public IActionResult EditPerson(int Id, string Name, int RoomId, int Bill, int TotalBill, IFormFile Image)
        {
            if (Name == null || Name == "" || _db.Persons.Where(a => a.Name == Name).Count() > 1)
            {
                //aM.addAlert("warning", "Wrong room name!");
                return ManagePerson();
            }

            Person person = _db.Persons.Find(Id);
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
            person.Bill = Bill;
            person.TotalBill = TotalBill;
            person.Updated = DateTime.Now;
            _db.Persons.Update(person);
            _db.SaveChanges();
            //aM.addAlert("success", "Room updated successfully!");
            return ManagePerson();
        }

        [HttpPost]
        public IActionResult DeletePerson(int Id)
        {
            _db.Persons.Remove(_db.Persons.Find(Id));
            _db.SaveChanges();
            //aM.addAlert("success", "Room created successfully!");
            return ManagePerson();
        }

        [HttpPost]
        public IActionResult CreateItem(string Name, string Description, int RoomId, int Count, string Price, IFormFile Image)
        {
            if (Name == null || Name == "" || _db.Persons.Where(a => a.Name == Name).Count() != 0)
            {
                //aM.addAlert("warning", "Wrong room name!");
                return ManageItem();
            }
            if (Description == null)
                Description = "";

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
            item.Price = Convert.ToDouble(Price.Replace(".", ","));
            item.Created = DateTime.Now;
            item.Updated = DateTime.Now;
            _db.Items.Add(item);
            _db.SaveChanges();
            //aM.addAlert("success", "Room created successfully!");
            return ManageItem();
        }

        [HttpPost]
        public IActionResult EditItem(int Id, string Name, string Description, int RoomId, int Count, string Price, IFormFile Image)
        {
            if (Name == null || Name == "" || _db.Persons.Where(a => a.Name == Name).Count() > 1)
            {
                //aM.addAlert("warning", "Wrong room name!");
                return ManageItem();
            }
            if (Description == null)
                Description = "";

            Item item = _db.Items.Find(Id);
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
            item.Price = Convert.ToDouble(Price.Replace(".", ","));
            item.Updated = DateTime.Now;
            _db.Items.Update(item);
            _db.SaveChanges();
            //aM.addAlert("success", "Room updated successfully!");
            return ManageItem();
        }

        [HttpPost]
        public IActionResult DeleteItem(int Id)
        {
            _db.Items.Remove(_db.Items.Find(Id));
            _db.SaveChanges();
            //aM.addAlert("success", "Room created successfully!");
            return ManageItem();
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
