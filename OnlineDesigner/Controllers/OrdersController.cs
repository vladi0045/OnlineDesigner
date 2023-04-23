using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineDesigner.Data;
using OnlineDesigner.Helpers;
using OnlineDesigner.Models;

namespace OnlineDesigner.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly OnlineDesignerContext _context;
        private readonly UserManager<User> _userManager;

        public OrdersController(OnlineDesignerContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
            _context.Order.Include(i => i.CartItems).ToList();
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
              return View(await _context.Order.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create(double price, string method)
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CartItems, Status,Price")] Order order, double price, string method)
        {

            if (ModelState.IsValid)
            {
                order.Status = false;
                var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");

                order.CartItems = cart;

                order.Price = price;

                order.PaymentMethod = method;

                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                List<Order> temp = new List<Order>();
                temp.Add(order);
                user.Orders = temp;
                _context.Update(user);
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ThankYou));
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CartItems, Status,Price")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Order == null)
            {
                return Problem("Entity set 'OnlineDesignerContext.Order'  is null.");
            }
            var order = await _context.Order.FindAsync(id);
            if (order != null)
            {
                _context.Order.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
          return _context.Order.Any(e => e.Id == id);
        }

        public async Task<IActionResult> ThankYou()
        {
            return View(await _context.Order.ToListAsync());
        }
        public async Task<IActionResult> YourOrders()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            return View(user.Orders);
        }
    }
}
