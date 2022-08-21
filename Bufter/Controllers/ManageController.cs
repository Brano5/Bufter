using Bufter.Data;
using Bufter.Model;
using Bufter.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bufter.Controllers
{
	public class ManageController : Controller
	{
        private readonly ApplicationDBContext _db;

		public ManageController(ApplicationDBContext db)
		{
			_db = db;
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
        public IActionResult CreateRoom(string Name)
        {

            return View("ManageRoom", _db.Rooms);
        }

        [HttpPost]
        public IActionResult EditRoom()
        {

            return View("ManageRoom", _db.Rooms);
        }

        [HttpPost]
        public IActionResult DeleteRoom(int Id)
        {

            return View("ManageRoom", _db.Rooms);
        }
    }
}
