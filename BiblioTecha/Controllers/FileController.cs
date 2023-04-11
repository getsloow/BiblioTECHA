using BiblioTecha.Areas.Identity.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
       public async Task<IActionResult> Upload (FileUploadViewModel model, int? bookId)
        {
            if (model.File == null || model.File.Length == 0)
            {
                ModelState.AddModelError("file", "Please select a file to upload.");
                return View(model);
            }
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.File.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }
            var file = new Models.FileModel
            {
                Name = model.File.Name,
                BookId = bookId,
                Location = fileName
            };
            _dbContext.Files.Add(file);
            await _dbContext.SaveChangesAsync();
            return View();
        }
    }
}
