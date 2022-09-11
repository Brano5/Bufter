using Bufter.Data;
using Bufter.Model;
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
            return View("log", new Tuple<IEnumerable<BuyLog>, IEnumerable<Log>>(_db.BuyLog, _db.Log));
        }
	}
}
