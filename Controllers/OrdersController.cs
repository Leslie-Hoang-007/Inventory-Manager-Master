using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventory_Manager.Data;
using Inventory_Manager.Models;
using Microsoft.AspNetCore.Authorization;

namespace Inventory_Manager.Controllers
{
    [Authorize]

    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Inventory' is null.");
            }

            var orders = await _context.Orders.ToListAsync();
            if(orders != null)
            {
                var totalQuantity = orders.Sum(item => item.Quantity);
                var totalValue = orders.Sum(item => item.TotalPrice);

                ViewBag.TotalQuantity = totalQuantity;
                ViewBag.TotalValue = totalValue.ToString("N2");
            }
            return View(orders);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }

        [Authorize(Roles = "Manager")]

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]

        public async Task<IActionResult> Create([Bind("Id,Date,Name,Quantity,Paid,Status,TotalPrice")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orders);
                await _context.SaveChangesAsync();
                // check if arrived order added
                if (orders.Status == "Arrived")
                {
                    UpdateInventory(orders);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(orders);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders.FindAsync(id);
            if (orders == null)
            {
                return NotFound();
            }
            return View(orders);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Name,Quantity,Paid,Status,TotalPrice")] Orders orders)
        {
            if (id != orders.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orders);
                    await _context.SaveChangesAsync();

                    if(orders.Status == "Arrived")
                    {
                        UpdateInventory(orders);
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdersExists(orders.Id))
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
            return View(orders);
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = "Manager")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Orders'  is null.");
            }
            var orders = await _context.Orders.FindAsync(id);
            if (orders != null)
            {
                _context.Orders.Remove(orders);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdersExists(int id)
        {
          return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        // Helper method to update the inventory when the status is "Arrived"
        private void UpdateInventory(Orders order)
        {
            if (_context?.Products != null)
            {
                // Extract the Name property before using it in LINQ
                string orderName = order.Name;

                // Case-insensitive comparison
                var orderItem = _context.Products.FirstOrDefault(p =>
                    string.Equals(p.Name, orderName));

                if (orderItem != null)
                {
                    // Update existing item
                    orderItem.Quantity += order.Quantity;
                    _context.Update(orderItem);
                }
                else
                {
                    // Create a new item if it doesn't exist
                    var newInventoryItem = new Products
                    {
                        Name = orderName,
                        Quantity = order.Quantity,
                        Paid = order.Paid,
                        Status = order.Status,
                        TotalPrice = order.TotalPrice,
                        Date = DateTime.Now
                    };

                    _context.Products.Add(newInventoryItem);
                }

                _context.SaveChanges();
            }
        }
    }
}
