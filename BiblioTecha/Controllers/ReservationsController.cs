﻿using BiblioTecha.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BiblioTecha.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ReservationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager=userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var reservations = _context.ReservationModel.Include(a => a.Book).ToList();

            return View(await _context.ReservationModel.Include(a => a.Book).ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Confirm (int? id)
        {
            var reservation = await _context.ReservationModel
                .Include(r => r.Book).FirstOrDefaultAsync(i => i.Id == id);
            if (reservation?.ReservationStatus == 0)
            {
                reservation.ReservationStatus = 1;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Free(int? id)
        {
            var reservation = await _context.ReservationModel
                .Include(r => r.Book).FirstOrDefaultAsync(i => i.Id == id);
            if (reservation != null) { 
                _context.ReservationModel.Remove(reservation);
                reservation.Book.Available++;
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == reservation.UserEmail);
                if (user != null)
                   
                {
                    if (reservation.ExpectedReturnDate < DateTime.UtcNow)
                    {
                        user.UserScore--;
                    }
                    else
                    {
                        user.UserScore++;
                    }
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}
