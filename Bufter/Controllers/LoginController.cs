using Bufter.Data;
using Bufter.Model;
using Bufter.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Xml.Linq;
using System.Globalization;
using System.Net.Sockets;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;

namespace Bufter.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDBContext _db;
        private readonly IWebHostEnvironment env;
        private readonly AlertManager aM;
        private readonly LogManager _logManager;
        private readonly NavigationManager navM;

        public LoginController(ApplicationDBContext db, IWebHostEnvironment environment, AlertManager alertManager, LogManager logManager)
        {
            _db = db;
            env = environment;
            aM = alertManager;
            _logManager = logManager;
        }

        public ActionResult Index()
        {
            if(HttpContext.User.Identity != null && HttpContext.User.Identity.Name != null)
            {
                return RedirectToAction("Index", "User");
            }

            return View("../Security/Login");
        }

        [HttpPost]
        public IActionResult Login(string Name, string Password)
        {
            if (Name == null || Name == "" || _db.Rooms.Where(a => a.Name == Name).Count() > 1)
            {
                //aM.addAlert("warning", "Wrong room name!");
                return RedirectToAction("Index", "Login");
            }
            if (Password == null || Password == "")
            {
                return RedirectToAction("Index", "Login");
            }

            User user = _db.Users.Where(a => a.Name == Name).Where(b => b.Password == Password).FirstOrDefault();
            if (user != null)
            {
                var claims = new List<Claim>{ new Claim(ClaimTypes.Name, user.Name) };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties();

                HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                _logManager.addLog("INFO", "Login User " + Name + " by " + Request.Cookies["Person"], HttpContext);

                @TempData["Info"] = "User legged in";
                //aM.addAlert("success", "Room updated successfully!");
                return RedirectToAction("Index", "User");
            }

            _logManager.addLog("INFO", "Login User Failed " + Name + " by " + Request.Cookies["Person"], HttpContext);

            aM.addAlert("success", "Room updated successfully!");
            return RedirectToAction("Index", "Login");
        }

        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync();

            _logManager.addLog("INFO", "LogOut User", HttpContext);

            @TempData["Info"] = "Succesfull LogOut.";

            return RedirectToAction("Index", "Login");
        }
    }
}
