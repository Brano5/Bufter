using Bufter.Data;
using Bufter.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bufter.Controllers
{
	public class LogController : Controller
	{
        private readonly ApplicationDBContext _db;

        public LogController(ApplicationDBContext db)
        {
            _db = db;
        }

        public IActionResult Index()
		{
            return BuyLog();
        }

        public IActionResult Log()
        {
            return View("Log", _db.Log);
        }

        public IActionResult BuyLog()
        {
            return View("BuyLog", _db.BuyLog);
        }
    }
}
