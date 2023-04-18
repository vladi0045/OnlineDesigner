using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OnlineDesigner.Data;

namespace OnlineDesigner.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class TypesController : Controller
    {
        private readonly OnlineDesignerContext _context;

        public TypesController(OnlineDesignerContext context)
        {
            _context = context;
        }

        public IEnumerable<Models.Type> TypeList()
        {
            return _context.Type.AsEnumerable();
        }

        // GET: Types
        public async Task<IActionResult> Index()
        {
              return View(await _context.Type.ToListAsync());
        }

        // GET: Types/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Type == null)
            {
                return NotFound();
            }

            var @type = await _context.Type
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@type == null)
            {
                return NotFound();
            }

            return View(@type);
        }

        // GET: Types/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Types/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Models.Type @type)
        {
            if (ModelState.IsValid)
            {
                @type.Id=Guid.NewGuid().ToString(); 
                _context.Add(@type);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@type);
        }

        // GET: Types/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Type == null)
            {
                return NotFound();
            }

            var @type = await _context.Type.FindAsync(id);
            if (@type == null)
            {
                return NotFound();
            }
            return View(@type);
        }

        // POST: Types/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name")] Models.Type @type)
        {
            if (id != @type.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@type);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeExists(@type.Id))
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
            return View(@type);
        }

        // GET: Types/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Type == null)
            {
                return NotFound();
            }

            var @type = await _context.Type
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@type == null)
            {
                return NotFound();
            }

            return View(@type);
        }

        // POST: Types/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Type == null)
            {
                return Problem("Entity set 'OnlineDesignerContext.Type'  is null.");
            }
            var @type = await _context.Type.FindAsync(id);
            if (@type != null)
            {
                _context.Type.Remove(@type);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypeExists(string id)
        {
          return _context.Type.Any(e => e.Id == id);
        }
    }
}
