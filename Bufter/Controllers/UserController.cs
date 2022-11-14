﻿using Bufter.Data;
using Bufter.Model;
using Bufter.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Xml.Linq;
using System.Globalization;
using Bufter.Helpers;

namespace Bufter.Controllers
{
	public class UserController : Controller
	{
        private readonly ApplicationDBContext _db;
        private readonly LogManager _logManager;

        public UserController(ApplicationDBContext db, LogManager logManager)
		{
			_db = db;
            _logManager = logManager;
        }

		public ActionResult Index()
		{
			return View("../Security/User", _db.Users);
		}

        [HttpPost]
        public IActionResult Create(string Name, string Password)
        {
            if (Name == null || Name == "" || _db.Users.Where(a => a.Name == Name).Count() != 0)
            {
                @TempData["Warning"] = "Wrong name!";

                return Index();
            }
            if (Password == null || Password == "")
            {
                return Index();
            }

            User user = new User();
            user.Name = Name;
            user.Password = CustomHelper.HashPassword(Password, Name);
            user.Created = DateTime.Now;
            user.Updated = DateTime.Now;
            _db.Users.Add(user);
            _db.SaveChanges();

            _logManager.addLog("INFO", "Created User " + Name + " by " + Request.Cookies["Person"], HttpContext);
            @TempData["Info"] = "Succesfull created!";
            
            return Index();
        }

        [HttpPost]
        public IActionResult Edit(int Id, string Name, string Password)
        {
            if (Name == null || Name == "" || _db.Rooms.Where(a => a.Name == Name).Count() > 1)
            {
                @TempData["Warning"] = "Wrong name!";
                return Index();
            }

            User? user = _db.Users.Find(Id);
            if (user == null)
            {
                return Index();
            }
            user.Name = Name;
            if (Password != null && Password != "")
            {
                user.Password = CustomHelper.HashPassword(Password, user.Name);
            }
            user.Updated = DateTime.Now;
            _db.Users.Update(user);
            _db.SaveChanges();

            _logManager.addLog("INFO", "Edited User " + Name + " by " + Request.Cookies["Person"], HttpContext);
            @TempData["Info"] = "Succesfull updated!";
            
            return Index();
        }

        [HttpPost]
        public IActionResult Delete(int Id)
        {
            _db.Users.Remove(_db.Users.Find(Id));
            _db.SaveChanges();

            _logManager.addLog("INFO", "Deleted User " + Id + " by " + Request.Cookies["Person"], HttpContext);
            @TempData["Info"] = "Succesfull deleted!";
            
            return Index();
        }
    }
}
