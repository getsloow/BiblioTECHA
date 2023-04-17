using BiblioTecha.Areas.Identity.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BiblioTecha.Controllers
{
    public class FileController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

    public FileController(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
    {
        _dbContext = dbContext;
        _webHostEnvironment=webHostEnvironment;
    }


        [HttpPost]
        public async Task<IActionResult> Upload(FileUploadViewModel model, int? bookId)
        {
            if (model.File == null || model.File.Length == 0)
            {
                ModelState.AddModelError("file", "Please select a file to upload.");
                return View(model);
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.File.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            // Check if a file with the same name already exists
            var existingFile = _dbContext.Files.FirstOrDefault(f => f.Name == model.File.FileName);
            if (existingFile != null)
            {
                var existingFilePath = Path.Combine(uploadsFolder, existingFile.Location);
                if (System.IO.File.Exists(existingFilePath))
                {
                    System.IO.File.Delete(existingFilePath);
                }

                existingFile.Location = fileName;
                _dbContext.Files.Update(existingFile);
            }
            else
            {
                var file = new Models.FileModel
                {
                    Name = model.File.FileName,
                    BookId = bookId,
                    Location = fileName
                };
                _dbContext.Files.Add(file);
            }

            await _dbContext.SaveChangesAsync();

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

            if (bookId != null)
            {
                return RedirectToAction("Details", "BookModels", new { id = bookId });
            }

            return View();
        }
        public IActionResult Download(int id)
        {
            var file = _dbContext.Files.FirstOrDefault(f => f.Id == id);
            if (file == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", file.Location);
            if (filePath == null)
            {
                return NotFound();
            }
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            return File(fileStream, "application/octet-stream", file.Name);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int fileId)
        {
            var file = await _dbContext.Files.FindAsync(fileId);
          

            if (file == null)
            {
                return NotFound();
            }
         

            // Delete the file from the file system
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", file.Location);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            // Delete the file from the database
            _dbContext.Files.Remove(file);
            await _dbContext.SaveChangesAsync();

            if (file.BookId != null)
            {
                return RedirectToAction("Details", "BookModels", new { id = file.BookId });
            }
            else
                return View();
        }
    }
}
