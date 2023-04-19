using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineDesigner.Data;
using OnlineDesigner.Models;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace OnlineDesigner.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ItemsController : Controller
    {
        private readonly OnlineDesignerContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public IEnumerable<Item> ItemList()
        {
            return _context.Item.AsEnumerable();
        }

        public ItemsController(OnlineDesignerContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
            _context.Item.Include(i => i.Type).ToList();
            _context.Type.Include(t => t.Item).ToList();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string id)
        {
            ViewBag.Id = id;
            return View(await _context.Item.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Item == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        public IActionResult Create()
        {
            ViewBag.Types = _context.Type
                     .Select(selector: i => new SelectListItem
                     {
                         Value = i.Id,
                         Text = i.Name
                     }).ToList();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,TypeId,Sex,Price,Description,Size,Img")] Item item, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                var fileName = GetUniqueFileName(Image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", fileName);
                item.Img = fileName;
                using (var fileSrteam = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(fileSrteam);
                }

                ViewBag.Types = _context.Type
                     .Select(selector: i => new SelectListItem
                     {
                         Value = i.Id,
                         Text = i.Name
                     }).ToList();

                Models.Type typeFromDB = _context.Type.Find(item.TypeId);

                if (typeFromDB != null)
                {
                    item.Type = typeFromDB;
                }

                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Types = _context.Type
                     .Select(selector: i => new SelectListItem
                     {
                         Value = i.Id,
                         Text = i.Name
                     }).ToList();

            if (id == null || _context.Item == null)
            {
                return NotFound();
            }

            var item = await _context.Item.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,TypeId,Sex,Price,Description,Size,Img")] Item item, IFormFile Image)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var fileName = GetUniqueFileName(Image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", fileName);
                item.Img = fileName;
                using (var fileSrteam = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(fileSrteam);
                }

                ViewBag.Types = _context.Type
                     .Select(selector: i => new SelectListItem
                     {
                         Value = i.Id,
                         Text = i.Name
                     }).ToList();

                Models.Type typeFromDB = _context.Type.Find(item.TypeId);

                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.Id))
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
            return View(item);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Item == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Item == null)
            {
                return Problem("Entity set 'OnlineDesignerContext.Item'  is null.");
            }
            var item = await _context.Item.FindAsync(id);
            if (item != null)
            {
                _context.Item.Remove(item);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.Id == id);
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
