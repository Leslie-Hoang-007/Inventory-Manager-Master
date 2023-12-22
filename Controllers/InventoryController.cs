using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventory_Management.Models;
using Inventory_Manager.Data;
using Inventory_Manager.Models;

namespace Inventory_Manager.Controllers
{
    public class InventoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Inventory
        public async Task<IActionResult> Index()
        {
            if (_context.Inventory == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Inventory' is null.");
            }

            var inventories = await _context.Inventory.ToListAsync();

            if (inventories != null)
            {
                // Calculate total quantity and total value
                var totalQuantity = inventories.Sum(item => item.Quantity);
                var totalValue = inventories.Sum(item => item.TotalPrice);

                ViewBag.TotalQuantity = totalQuantity;
                ViewBag.TotalValue = totalValue.ToString("N2");
            }

            return View(inventories);
        }


        // GET: Inventory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Inventory == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // GET: Inventory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Inventory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Name,Quantity,Paid,Status,TotalPrice")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inventory);
                await _context.SaveChangesAsync();
                // check if arrived and add
                if (inventory.Status == "Arrived")
                {
                    UpdateInventory(inventory);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(inventory);
        }

        // GET: Inventory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Inventory == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventory.FindAsync(id);
            if (inventory == null)
            {
                return NotFound();
            }
            return View(inventory);
        }

        // POST: Inventory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Name,Quantity,Paid,Status,TotalPrice")] Inventory inventory)
        {
            if (id != inventory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventory);
                    await _context.SaveChangesAsync();

                    if (inventory.Status == "Arrived")
                    {
                        UpdateInventory(inventory);
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryExists(inventory.Id))
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
            return View(inventory);
        }

        // GET: Inventory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Inventory == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // POST: Inventory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Inventory == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Inventory'  is null.");
            }
            var inventory = await _context.Inventory.FindAsync(id);
            if (inventory != null)
            {
                _context.Inventory.Remove(inventory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventoryExists(int id)
        {
          return (_context.Inventory?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // Helper method to update the inventory when the status is "Arrived"
        private void UpdateInventory(Inventory inventory)
        {
            if (_context?.Products != null)
            {
                // Extract the Name property before using it in LINQ
                string inventoryName = inventory.Name;

                // Case-insensitive comparison
                var inventoryItem = _context.Products.FirstOrDefault(p =>
                    string.Equals(p.Name, inventoryName));

                if (inventoryItem != null)
                {
                    // Update existing item
                    inventoryItem.Quantity += inventory.Quantity;
                    _context.Update(inventoryItem);
                }
                else
                {
                    // Create a new item if it doesn't exist
                    var newInventoryItem = new Products
                    {
                        Name = inventoryName,
                        Quantity = inventory.Quantity,
                        Paid = inventory.Paid,
                        Status = inventory.Status,
                        TotalPrice = inventory.TotalPrice,
                        Date = DateTime.Now
                };

                    _context.Products.Add(newInventoryItem);
                }

                _context.SaveChanges();
            }
        }
    }
}
