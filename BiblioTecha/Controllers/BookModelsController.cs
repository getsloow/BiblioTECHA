using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BiblioTecha.Areas.Identity.Data;
using BiblioTecha.Models;
using Microsoft.AspNetCore.Identity;
using BiblioTecha.Controllers;
using BiblioTecha.Migrations;

namespace BiblioTecha.Controllers
{
    public class BookModelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookModelsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager=userManager;
           
        }
        
    // GET: BookModels
    public async Task<IActionResult> Index()
        {
             var user = await _userManager.GetUserAsync(User);
            var genres = _context.BookModel.Select(x => x.Genre).Distinct().ToList();
            ViewBag.Genres = genres;
            if (user!=null)
            {
                ViewBag.IsAdmin = user.isAdmin;

            }
            return View(await _context.BookModel.ToListAsync());
        }

        //  [HttpGet("/BookModels/Genre")]
        public async Task<IActionResult> Genre(string gen)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user!=null)
            {
                ViewBag.IsAdmin = user.isAdmin;
            }
            var books = await _context.BookModel.Where(x => x.Genre == gen).ToListAsync();
            return View(books);
        }

        // GET: BookModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BookModel == null)
            {
                return NotFound();
            }

            var bookModel = await _context.BookModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookModel == null)
            {
                return NotFound();
            }

            return View(bookModel);
        }

        // GET: BookModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BookModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Author,Available,ImageLink,Genre")] BookModel bookModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookModel);
        }

        // GET: BookModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BookModel == null)
            {
                return NotFound();
            }

            var bookModel = await _context.BookModel.FindAsync(id);
            if (bookModel == null)
            {
                return NotFound();
            }
            return View(bookModel);
        }

        // POST: BookModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Author,Available,ImageLink,Genre")] BookModel bookModel)
        {
            if (id != bookModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookModelExists(bookModel.Id))
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
            return View(bookModel);
        }

        // GET: BookModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BookModel == null)
            {
                return NotFound();
            }

            var bookModel = await _context.BookModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookModel == null)
            {
                return NotFound();
            }

            return View(bookModel);
        }

        // POST: BookModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BookModel == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BookModel'  is null.");
            }
            var bookModel = await _context.BookModel.FindAsync(id);
            if (bookModel != null)
            {
                _context.BookModel.Remove(bookModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ChangeAvailability(int? id)
        {
            if (id == null || _context.BookModel == null)
            {
                return NotFound();
            }

            var bookModel = await _context.BookModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookModel == null)
            {
                return NotFound();
            }

            return View(bookModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeAvailability(int id)
        //{
        //    var book = _context.BookModel.FirstOrDefault(x => x.Id == id);
        //    if (book != null)
        //    {
        //        book.Available = !book.Available;
        //        _context.SaveChanges();
        //    }
        //    return RedirectToAction(nameof(Index));
        //}
          {
            if (_context.BookModel == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BookModel'  is null.");
            }
            var bookModel = await _context.BookModel.FindAsync(id);
            if (bookModel != null)
            {
                bookModel.Available = !bookModel.Available;
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool BookModelExists(int id)
        {
          return _context.BookModel.Any(e => e.Id == id);
        }
    }
}
