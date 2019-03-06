using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalWebApp.Models;
using Microsoft.AspNetCore.Http;
using FinalWebApp.Dto;
using System.IO;

namespace FinalWebApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly MyContext _context;

        public OrdersController(MyContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            if (Globals.getConnectedUser(HttpContext.Session) == null)
            {
                return NotFound();
            }
            if (Globals.isAdminConnected(HttpContext.Session))
            {
                return View(await _context.Order.Include(x => x.Hotel).ToListAsync());
            }
            else
            {
                return View(await _context.Order.Include( x=>x.Hotel)
                    .Where(o => o.UserId == Globals.getConnectedUser(HttpContext.Session).Id)
                    .ToListAsync());
            }
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.Include(x=>x.Hotel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null || (Globals.getConnectedUser(HttpContext.Session) == null || (!Globals.isAdminConnected(HttpContext.Session) &&
                   order.UserId != Globals.getConnectedUser(HttpContext.Session).Id)))
            {
                return NotFound();
            }

            return View(order);
        }

        //// GET: Orders/Create
        //public IActionResult Create()
        //{
        //    if (Globals.getConnectedUser(HttpContext.Session) != null)
        //    {
        //        return View();
        //    }

        //    return NotFound();
        //}

        //// POST: Orders/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,Email,CheckInDate,CheckOutDate,UserId,HotelId")] Order order)
        //{
        //    if (Globals.getConnectedUser(HttpContext.Session) != null)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            _context.Add(order);
        //            await _context.SaveChangesAsync();
        //            return RedirectToAction(nameof(Index));
        //        }
        //        return View(order);
        //    }
        //    return NotFound();
        //}

        //// GET: Orders/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var order = await _context.Order.FindAsync(id);
        //    if (order == null || (Globals.getConnectedUser(HttpContext.Session) == null || (!Globals.isAdminConnected(HttpContext.Session) &&
        //           order.UserId != Globals.getConnectedUser(HttpContext.Session).Id)))
        //    {
        //        return NotFound();
        //    }
        //    return View(order);
        //}

        //// POST: Orders/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,CheckInDate,CheckOutDate,UserId,HotelId")] Order order)
        //{
        //    if (id != order.Id || (Globals.getConnectedUser(HttpContext.Session) == null || (!Globals.isAdminConnected(HttpContext.Session) &&
        //           order.UserId != Globals.getConnectedUser(HttpContext.Session).Id)))
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(order);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!OrderExists(order.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(order);
        //}

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id, int userId)
        {
            if (id == null || (Globals.getConnectedUser(HttpContext.Session) == null || (!Globals.isAdminConnected(HttpContext.Session) &&
                   userId != Globals.getConnectedUser(HttpContext.Session).Id)))
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int userId)
        {
            if (Globals.getConnectedUser(HttpContext.Session) == null || (!Globals.isAdminConnected(HttpContext.Session) &&
                   userId != Globals.getConnectedUser(HttpContext.Session).Id))
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.Id == id);
        }

		[HttpPost]
		public async Task<IActionResult> Confirm(OrderModel orderModel)
		{
			var userId = Globals.getConnectedUser(HttpContext.Session)?.Id ?? -1;
			var userEmail = Globals.getConnectedUser(HttpContext.Session)?.Email ?? "";
			var userName = Globals.getConnectedUser(HttpContext.Session)?.Name ?? "";

			if (userId == -1)
			{
				return StatusCode(StatusCodes.Status401Unauthorized, "Please log in or register");
			}

			var order = new Order()
			{
				UserId = userId,
				Email = userEmail,
				Name = userName,
				HotelId = orderModel.HotelId,
				CheckInDate = orderModel.CheckInDate,
				CheckOutDate = orderModel.CheckOutDate
			};

            await _context.Order.AddAsync(order);

            await _context.SaveChangesAsync();

			if (orderModel.isAi)
			{
				SaveToTrainData(orderModel);
			}

            return Ok(order.Id);
        }

		private void SaveToTrainData(OrderModel orderModel)
		{
			var price = _context.Hotel.FirstOrDefault(x => x.Id == orderModel.HotelId)?.Price;
			var line = "\n" + string.Join(",", orderModel.userAge, orderModel.userGender, orderModel.userProfession, orderModel.userFamilyStatus, orderModel.userHobby, orderModel.userPurpose, price);
			System.IO.File.AppendAllTextAsync("TrainingData.csv", line);
		}

		public async Task<IActionResult> Summery(int id)
        {
            Order order = await _context.Order.Include(o => o.Hotel)
                .FirstOrDefaultAsync(h => h.Id == id);

            return View("SummeryView", order);
        }
    }
}
