using Bufter.Data;
using Bufter.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml.Linq;

namespace Bufter
{
    public class LogManager
    {
        private readonly ApplicationDBContext _db;

        public LogManager(ApplicationDBContext db)
        {
            _db = db;
        }

        public void addBuyLog(string room, string person, string item, string description, HttpContext context)
        {
            BuyLog buyLogDb = new BuyLog();
            buyLogDb.Room = room;
            buyLogDb.Person = person;
            buyLogDb.Item = item;
            buyLogDb.Description = description;
            buyLogDb.Created = DateTime.Now;
            buyLogDb.IpAddress = getIPAddress(context);
            buyLogDb.PcName = getName();
            buyLogDb.UserAgent = getUserAgent(context);
            buyLogDb.UniKey = getUniKey(context);
            _db.BuyLog.Add(buyLogDb);
            _db.SaveChanges();
        }

        public void addLog(string name, string description, HttpContext context)
        {
            Log logDb = new Log();
            logDb.Name = name;
            logDb.Description = description;
            logDb.Created = DateTime.Now;
            logDb.IpAddress = getIPAddress(context);
            logDb.PcName = getName();
            logDb.UserAgent = getUserAgent(context);
            logDb.UniKey = getUniKey(context);
            _db.Log.Add(logDb);
            _db.SaveChanges();
        }

        public string getIPAddress(HttpContext context)
        {
            return context.Connection.RemoteIpAddress.ToString();
        }

        public string getName()
        {
            return System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
        }

        public string getUserAgent(HttpContext context)
        {
            return context.Request.Headers["User-Agent"].ToString();
        }

        public string getUniKey(HttpContext context)
        {
            if (context.Request.Cookies["UniKey"] == null)
            {
                context.Response.Cookies.Append("UniKey", Guid.NewGuid().ToString(), new CookieOptions { Expires = DateTime.Now.AddYears(10) });
            }
            return context.Request.Cookies["UniKey"];
        }
    }
}
