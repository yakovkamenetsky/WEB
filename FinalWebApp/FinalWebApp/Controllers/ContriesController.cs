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
    public class ContriesController : Controller
    {
        private readonly MyContext _context;

        public ContriesController(MyContext context)
        {
            _context = context;
        }

        // GET: Contries
        public async Task<IActionResult> Index()
        {
            return View(await _context.Contry.ToListAsync());
        }

        // GET: Contries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contry = await _context.Contry
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contry == null)
            {
                return NotFound();
            }

            return View(contry);
        }

        // GET: Contries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Contry contry)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contry);
        }

        // GET: Contries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contry = await _context.Contry.FindAsync(id);
            if (contry == null)
            {
                return NotFound();
            }
            return View(contry);
        }

        // POST: Contries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Contry contry)
        {
            if (id != contry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContryExists(contry.Id))
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
            return View(contry);
        }

        // GET: Contries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contry = await _context.Contry
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contry == null)
            {
                return NotFound();
            }

            return View(contry);
        }

        // POST: Contries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contry = await _context.Contry.FindAsync(id);
            _context.Contry.Remove(contry);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContryExists(int id)
        {
            return _context.Contry.Any(e => e.Id == id);
        }
    }
}
