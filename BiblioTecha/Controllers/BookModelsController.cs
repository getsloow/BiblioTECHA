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
using Microsoft.AspNetCore.Authorization;

namespace BiblioTecha.Controllers
{
    public class BookModelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public BookModelsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager=userManager;
            _roleManager = roleManager;
        }

        // GET: BookModels
        [Authorize]
    public async Task<IActionResult> Index()
        {
            var roleCreate = new IdentityRole { Name = "Teacher" };
            var result = await _roleManager.CreateAsync(roleCreate);

            var role = await _roleManager.FindByNameAsync("Teacher");
            if (role == null)
            {
                // Role does not exist, handle the error
            }

            var user = await _userManager.FindByEmailAsync("profesor@test.com");
            if (user == null)
            {
                // User does not exist, handle the error
            }
            else
            {
                var isInRole = await _userManager.IsInRoleAsync(user, "Teacher");
            }


            if (user != null && role != null)
            {
                var resultat = await _userManager.AddToRoleAsync(user, role.Name);

                if (resultat.Succeeded)
                {
                    Console.WriteLine("e admin");
                }
                else
                {
                    // Failed to add role to user, check the errors in result.Errors
                }
            }


            var genres = _context.BookModel.Select(x => x.Genre).Distinct().ToList();
            ViewBag.Genres = genres;
           
            return View(await _context.BookModel.ToListAsync());
        }

        //  [HttpGet("/BookModels/Genre")]
        public async Task<IActionResult> Genre(string gen)
        {
            var genres = _context.BookModel.Select(x => x.Genre).Distinct().ToList();
            ViewBag.Genres = genres;

            var books = await _context.BookModel.Where(x => x.Genre == gen).ToListAsync();
            return View(books);
        }

        // GET: BookModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _context.BookModel
                .Include(b => b.Author)
                .Include(b => b.File)
                .FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            var viewModel = new BookDetailsViewModel
            {
                Book = book,
                Author = book.Author,
                File = book.File
            };

            return View(viewModel);
        }

        public async Task<IActionResult> ReserveAsync(int? id)
        {
            if (id == null || _context.BookModel == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            var userScore = await _context.Users.Where(u => u.Email == user.Email).Select(u => u.UserScore).FirstOrDefaultAsync();
            ViewBag.UserScore = userScore;

            var bookModel = await _context.BookModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookModel == null)
            {
                return NotFound();
            }
            if (bookModel.Available > 0)
                
                return View(bookModel);
            else
                return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Reserve(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var userScore = await _context.Users.Where(u => u.Email == user.Email).Select(u => u.UserScore).FirstOrDefaultAsync();
            var book = await _context.BookModel.FirstOrDefaultAsync(m => m.Id == id);
            var reservation = new ReservationModel
            {
                BookId = id.Value,
                ExpectedReturnDate= DateTime.Now.AddDays(book.ReadingDays),
                Book = book,
                UserScore = userScore,
                ReservationStatus = 0,
                ReservationDate= DateTime.Now,
                UserEmail = User.Identity.Name
            };
            if (ModelState.IsValid)
            {
                book.Available--;
                _context.ReservationModel.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(id);
        }

        // GET: BookModels/Create
        public IActionResult Create()
        {
            var authors = _context.AuthorModel.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name }).ToList();

            ViewBag.Authors = authors;
            return View();
        }


        // POST: BookModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,AuthorId,Available,ImageLink,Genre,ReadingDays")] BookModel bookModel)
        {
            if (ModelState.IsValid)
            {
                var author = await _context.AuthorModel.FindAsync(bookModel.AuthorId);
                bookModel.Author = author;
                _context.Add(bookModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.AuthorModel, "Id", "Name", bookModel.AuthorId);
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
            var authors = _context.AuthorModel.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name }).ToList();

            ViewBag.Authors = authors;

            return View(bookModel);
        }

        // POST: BookModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,AuthorId,Available,ImageLink,Genre,ReadingDays")] BookModel bookModel)
        {
            if (id != bookModel.Id)
            {
                return NotFound();
            }

            var author = await _context.AuthorModel.FindAsync(bookModel.AuthorId);
            bookModel.Author = author;
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
      
          {
            if (_context.BookModel == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BookModel'  is null.");
            }
            var bookModel = await _context.BookModel.FindAsync(id);
            if (bookModel != null)
            {
                bookModel.Available--;
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
