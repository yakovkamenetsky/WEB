using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalWebApp.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace FinalWebApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly MyContext _context;
        private readonly object ClientScript;

        public UsersController(MyContext context)

        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            if (!Globals.isAdminConnected(HttpContext.Session))
            {
                return NotFound();
            }
            var myContext = _context.User.Include(u => u.City);
            HttpContext.Session.SetString("isUserAdmin", "false");
            return View(await myContext.ToListAsync());
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            if (!Globals.isAdminConnected(HttpContext.Session))
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.City, "Id", "Id");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Password,Birthday,Gender,CityId,Profession,FamilyStatus,IsAdmin")] User user)
        {
            if (!Globals.isAdminConnected(HttpContext.Session))
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.City, "Id", "Id", user.CityId);
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!Globals.isAdminConnected(HttpContext.Session) &&
                id != Globals.getConnectedUser(HttpContext.Session)?.Id)
            {
                return NotFound();
            }
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.City, "Id", "Name", user.CityId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Password,Birthday,Gender,CityId,Profession,FamilyStatus,IsAdmin")] User user)
        {
            if (!Globals.isAdminConnected(HttpContext.Session))
            {
                return NotFound();
            }
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.City, "Id", "Id", user.CityId);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!Globals.isAdminConnected(HttpContext.Session))
            {
                return NotFound();
            }
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

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!Globals.isAdminConnected(HttpContext.Session))
            {
                return NotFound();
            }
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
