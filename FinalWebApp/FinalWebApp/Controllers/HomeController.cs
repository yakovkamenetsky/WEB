﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FinalWebApp.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace FinalWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyContext _context;

        public HomeController(MyContext context)

        {
            _context = context;
        }


        [HttpGet]
        public IActionResult Register()
        {


            return View();
        }

        [HttpPost]
        public IActionResult Register(string email, string password, string confirmPassword)
        {
            if (!(String.IsNullOrWhiteSpace(email) || String.IsNullOrWhiteSpace(password)))
            {
                var user = _context.User.Where(x => x.Email.Equals(email));
                if (user.Any())
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "User is already exist!");
                }
                else if (!String.IsNullOrWhiteSpace(confirmPassword) && password.Equals(confirmPassword))
                {
                    User newUser = new User();
                    newUser.Email = email;
                    newUser.Password = password;

                    _context.User.Add(newUser);
                    _context.SaveChanges();

                    return Ok("User added successfuly!");
                }
            }

            return StatusCode(StatusCodes.Status401Unauthorized, "Enter Email and Password correctly!");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User usr)
        {
            if (!(usr.Email == null || usr.Email.Trim() == ""))
            {

                var user = _context.User.Where(x => x.Email.Equals(usr.Email.Trim()) && x.Password.Equals(usr.Password));
                if (user.Any())
                {
                    if (user.First().IsAdmin)
                    {
                        HttpContext.Session.SetString(Globals.ADMIN_SESSION_KEY, "true");
                    }

                    string jsonUser = JsonConvert.SerializeObject(usr);
                    HttpContext.Session.SetString(Globals.USER_SESSION_KEY, jsonUser);

                    return Ok(null);
                }
            }

            return StatusCode(StatusCodes.Status401Unauthorized, "Invalid Email or Password!");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.SetString(Globals.ADMIN_SESSION_KEY, "false");
            HttpContext.Session.Remove(Globals.USER_SESSION_KEY);

            return View("Index");
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .Include(u => u.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
