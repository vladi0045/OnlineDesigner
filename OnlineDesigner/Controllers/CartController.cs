using OnlineDesigner.Helpers;
using OnlineDesigner.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using OnlineDesigner.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace OnlineDesigner.Controllers
{
    [Authorize]
    [Route("cart")]
    public class CartController : Controller
    {
        private readonly OnlineDesignerContext _context;

        public CartController(OnlineDesignerContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _context.Design.Include(i => i.Item).ToList();
        }

        [Route("index")]
        public IActionResult Index()
        {
            if (SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart") != null)
            {
                var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
                ViewBag.cart = cart;
                ViewBag.total = cart.Sum(item => item.Design.Item.Price * item.Quantity);
            }
            else
            {
                ViewBag.cart = new List<CartItem>().AsEnumerable();
                ViewBag.total = 0;
            }

            return View();
        }

        [Route("buy/{id}")]
        public IActionResult Buy(int id, int quantity)
        {
            if (SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart") == null)
            {
                List<CartItem> cart = new List<CartItem>();
                cart.Add(new CartItem { Design = _context.Design.Find(id), Quantity = quantity });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
                int index = isExist(id);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new CartItem { Design = _context.Design.Find(id), Quantity = quantity });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("Index");
        }

        [Route("remove/{id}")]
        public IActionResult Remove(int id)
        {
            List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            int index = isExist(id);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
        }

        private int isExist(int id)
        {
            List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Design.Id.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }

    }
}
