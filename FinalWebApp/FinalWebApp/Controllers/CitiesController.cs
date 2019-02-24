using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalWebApp.Models;

namespace FinalWebApp.Controllers
{
    public class CitiesController : Controller
    {
        private readonly MyContext _context;

        public CitiesController(MyContext context)
        {
            _context = context;
        }

        // GET: Cities
        public async Task<IActionResult> Index()
        {
            if (!Globals.isAdminConnected(HttpContext.Session))
            {
                return NotFound();
            }
            var databaseContext = _context.City.Include(c => c.Country);
            return View(await databaseContext.ToListAsync());
        }

        // GET: Cities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!Globals.isAdminConnected(HttpContext.Session))
            {
                return NotFound();
            }
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.City.Include(c => c.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // GET: Cities/Create
        public async Task<IActionResult> Create()
        {
            if (!Globals.isAdminConnected(HttpContext.Session))
            {
                return NotFound();
            }
            var q = from c in _context.Country.Include(c => c.Name)
                    select new { Value = c.Id, Text = c.Name };

            ViewData["CountryId"] = new SelectList(await q.ToListAsync(), "Value", "Text");

            return View();
        }

        // POST: Cities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] City city)
        {
            if (!Globals.isAdminConnected(HttpContext.Session))
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.Add(city);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(city);
        }

        // GET: Cities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!Globals.isAdminConnected(HttpContext.Session))
            {
                return NotFound();
            }
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.City.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }

            ViewData["CountryId"] = new SelectList(_context.Country, "Id", "Name", city.Country);

            return View(city);
        }

        // POST: Cities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] City city)
        {
            if (!Globals.isAdminConnected(HttpContext.Session))
            {
                return NotFound();
            }
            if (id != city.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(city);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CityExists(city.Id))
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
            return View(city);
        }

        // GET: Cities/Delete/5
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

            var city = await _context.City.Include(c => c.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // POST: Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!Globals.isAdminConnected(HttpContext.Session))
            {
                return NotFound();
            }
            var city = await _context.City.FindAsync(id);
            _context.City.Remove(city);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CityExists(int id)
        {
            return _context.City.Any(e => e.Id == id);
        }
    }
}
