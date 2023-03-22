using Bufter.Data;
using Bufter.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bufter.Controllers
{
    public class ChartController : Controller
    {

        private readonly ApplicationDBContext _db;

        public ChartController(ApplicationDBContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View("chart", _db.BuyLog);
        }
    }
}
