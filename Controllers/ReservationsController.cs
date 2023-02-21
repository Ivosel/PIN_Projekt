using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PIN_Projekt.Data;
using PIN_Projekt.Models;

namespace PIN_Projekt.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
              return _context.Reservation != null ? 
                          View(await _context.Reservation.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Reservation'  is null.");
        }

        public ActionResult SpecificIndex()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var reservations = _context.Reservation.Where(r => r.Email == email)
                .Join(_context.Vehicle, r => r.VehicleId, v => v.Id, (r, v) => new ReservationViewModel
                {
                    Id = r.Id,
                    VehicleModel = v.Model,
                    VehicleMake = v.Make,
                    StartDate = r.BeginDate,
                    EndDate = r.ReturnDate,
                    Email = r.Email,
                    TotalPrice = r.TotalCost
                }).ToList();
            return View(reservations);
        }


        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reservation == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create

        [HttpPost]
        public ActionResult Create(int selectedVehicleId, DateTime startDate, DateTime endDate)
        {
            var vehicle = _context.Vehicle.Find(selectedVehicleId);
            if (vehicle == null)
            {
                return NotFound();
            }

            var reservation = new Reservation
            {
                VehicleId = vehicle.Id,
                BeginDate = startDate,
                ReturnDate = endDate,
                TotalCost = vehicle.DailyRate * (endDate - startDate).Days,
                Email = User.FindFirstValue(ClaimTypes.Email)
            };

            _context.Reservation.Add(reservation);
            _context.SaveChanges();

            return RedirectToAction(nameof(SpecificIndex));
        }
        public IActionResult CreateViewModel()
        {
            var model = new ReservationCreateViewModel();
            model.VehicleTypes = _context.Vehicle.Select(v => v.Type).Distinct().ToList();
            return View(model);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reservation == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VehicleId,BeginDate,ReturnDate,TotalCost")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
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
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Reservation == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reservation == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Reservation'  is null.");
            }
            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservation.Remove(reservation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
          return (_context.Reservation?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
