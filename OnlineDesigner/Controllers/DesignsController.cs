using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineDesigner.Data;
using OnlineDesigner.Models;

namespace OnlineDesigner.Controllers
{
    [Authorize]
    public class DesignsController : Controller
    {
        private readonly OnlineDesignerContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public DesignsController(OnlineDesignerContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _context.Design.Include(i => i.Item).ToList();
            this._hostEnvironment = hostEnvironment;
        }

        // GET: Designs
        public async Task<IActionResult> Index()
        {
              return View(await _context.Design.ToListAsync());
        }

        // GET: Designs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Design == null)
            {
                return NotFound();
            }

            var design = await _context.Design
                .FirstOrDefaultAsync(m => m.Id == id);
            if (design == null)
            {
                return NotFound();
            }

            return View(design);
        }

        public IActionResult Create(int id)
        {
            ViewBag.Item = id;

            return View();
        }

        // POST: Designs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ItemId,Size,Color,Img")] Design design, IFormFile Image, int id, int quantity)
        {
            if (ModelState.IsValid)
            {
                design.ItemId = id;
                Item itemFromDB = _context.Item.Find(design.ItemId);

                if (itemFromDB != null)
                {
                    design.Item = itemFromDB;
                }

                ViewBag.Item = id;

                var fileName = GetUniqueFileName(Image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", fileName);
                design.Img = fileName;
                using (var fileSrteam = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(fileSrteam);
                }

                _context.Add(design);
                await _context.SaveChangesAsync();
                return RedirectToAction("Buy", "Cart", new { id = design.Id, quantity = quantity });
            }
            return View(design);
        }

        // GET: Designs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Design == null)
            {
                return NotFound();
            }

            var design = await _context.Design.FindAsync(id);
            if (design == null)
            {
                return NotFound();
            }
            return View(design);
        }

        // POST: Designs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Size,Color,Img")] Design design)
        {
            if (id != design.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(design);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DesignExists(design.Id))
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
            return View(design);
        }

        // GET: Designs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Design == null)
            {
                return NotFound();
            }

            var design = await _context.Design
                .FirstOrDefaultAsync(m => m.Id == id);
            if (design == null)
            {
                return NotFound();
            }

            return View(design);
        }

        // POST: Designs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Design == null)
            {
                return Problem("Entity set 'OnlineDesignerContext.Design'  is null.");
            }
            var design = await _context.Design.FindAsync(id);
            if (design != null)
            {
                _context.Design.Remove(design);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DesignExists(int id)
        {
          return _context.Design.Any(e => e.Id == id);
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }
    }
}
