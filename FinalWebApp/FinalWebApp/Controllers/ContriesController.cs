﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalWebApp.Models;
using Microsoft.AspNetCore.Http;

namespace FinalWebApp.Controllers
{
	public class CountriesController : Controller
	{
		private readonly MyContext _context;

		public CountriesController(MyContext context)
		{
			_context = context;
		}

		// GET: Countries
		public async Task<IActionResult> Index()
		{
			if (!Globals.isAdminConnected(HttpContext.Session))
			{
				return NotFound();
			}
			var myContext = _context.Country;

			return View(await myContext.ToListAsync());
		}




		// GET: Countries/Details/5
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

			var country = await _context.Country
				.FirstOrDefaultAsync(m => m.Id == id);
			if (country == null)
			{
				return NotFound();
			}

			return View(country);
		}

		// GET: Countries/Create
		public IActionResult Create()
		{
			if (!Globals.isAdminConnected(HttpContext.Session))
			{
				return NotFound();
			}
			return View();
		}

		// POST: Countries/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Name")] Country country)
		{
			if (!Globals.isAdminConnected(HttpContext.Session))
			{
				return NotFound();
			}
			if (ModelState.IsValid)
			{
				_context.Add(country);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(country);
		}

		// GET: Countries/Edit/5
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

			var country = await _context.Country.FindAsync(id);
			if (country == null)
			{
				return NotFound();
			}
			return View(country);
		}

		// POST: Countries/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Country country)
		{
			if (!Globals.isAdminConnected(HttpContext.Session))
			{
				return NotFound();
			}
			if (id != country.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(country);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!CountryExists(country.Id))
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
			return View(country);
		}

		// GET: Countries/Delete/5
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

			var country = await _context.Country
				.FirstOrDefaultAsync(m => m.Id == id);
			if (country == null)
			{
				return NotFound();
			}

			return View(country);
		}

		// POST: Countries/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (!Globals.isAdminConnected(HttpContext.Session))
			{
				return NotFound();
			}
			var country = await _context.Country.FindAsync(id);
			_context.Country.Remove(country);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool CountryExists(int id)
		{
			return _context.Country.Any(e => e.Id == id);
		}


		public async Task<IActionResult> SearchCities(string countryName)
		{
			if (!Globals.isAdminConnected(HttpContext.Session))
			{
				return NotFound();
			}
			var myContext = _context.Country;
			HttpContext.Session.SetString("isUserAdmin", "false");

			if (!String.IsNullOrEmpty(countryName))
			{
				var q = from p in _context.Country.Where(x => x.Name.Contains(countryName))
						join m in _context.City on p.Id equals m.CountryId
						select new City()
						{
							Id = m.Id,
							Name = m.Name,
							Country = m.Country,
							CountryId = m.CountryId,
							Hotels = m.Hotels,
							Users = m.Users
						};
				return View("~/Views/Countries/ViewCities.cshtml", await q.ToListAsync());
			}

			return View("Index",await myContext.ToListAsync());
		}
	}
}
