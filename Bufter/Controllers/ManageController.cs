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
            return View("ManagePerson", _db.Persons);
        }

        public IActionResult ManageItem()
        {
            return View("ManageItem", _db.Items);
        }

        [HttpPost]
        public IActionResult CreateRoom(string Name, string Description, IFormFile Image)
        {
            if (Name == null || Name == "" || _db.Rooms.Where(a => a.Name == Name).Count() != 0)
            {
                //aM.addAlert("warning", "Wrong room name!");
                return View("ManageRoom", _db.Rooms);
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
            return View("ManageRoom", _db.Rooms);
        }

        [HttpPost]
        public IActionResult EditRoom(int Id, string Name, string Description, IFormFile Image)
        {
            if (Name == null || Name == "" || _db.Rooms.Where(a => a.Name == Name).Count() > 1)
            {
                //aM.addAlert("warning", "Wrong room name!");
                return View("ManageRoom", _db.Rooms);
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
            return View("ManageRoom", _db.Rooms);
        }

        [HttpPost]
        public IActionResult DeleteRoom(int Id)
        {
            _db.Rooms.Remove(_db.Rooms.Find(Id));
            _db.SaveChanges();
            //aM.addAlert("success", "Room created successfully!");
            return View("ManageRoom", _db.Rooms);
        }

        [HttpPost]
        public IActionResult CreatePerson(string Name, int RoomId, IFormFile Image)
        {
            if (Name == null || Name == "" || _db.Persons.Where(a => a.Name == Name).Count() != 0)
            {
                //aM.addAlert("warning", "Wrong room name!");
                return View("ManagePerson", _db.Persons);
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
            return View("ManagePerson", _db.Persons);
        }

        [HttpPost]
        public IActionResult EditPerson(int Id, string Name, int RoomId, int Bill, int TotalBill, IFormFile Image)
        {
            if (Name == null || Name == "" || _db.Persons.Where(a => a.Name == Name).Count() > 1)
            {
                //aM.addAlert("warning", "Wrong room name!");
                return View("ManagePerson", _db.Persons);
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
            return View("ManagePerson", _db.Persons);
        }

        [HttpPost]
        public IActionResult DeletePerson(int Id)
        {
            _db.Persons.Remove(_db.Persons.Find(Id));
            _db.SaveChanges();
            //aM.addAlert("success", "Room created successfully!");
            return View("ManagePerson", _db.Persons);
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
