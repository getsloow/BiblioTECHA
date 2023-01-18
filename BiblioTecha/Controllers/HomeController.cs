﻿using BiblioTecha.Areas.Identity.Data;
using BiblioTecha.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BiblioTecha.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context=context;
        }

        public async Task<IActionResult>  Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user!=null) 
            { 
            ViewBag.IsAdmin = user.isAdmin;
            }
            return View(await _context.BookModel.ToListAsync());
            //return View();
        }

        public async Task<IActionResult> No()
        {
            return View();
        }
        public async Task<IActionResult> Privacy()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
                if (user.isAdmin == true)
                {
                    return View();
                }
            return Unauthorized();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}