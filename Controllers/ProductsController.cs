﻿using System;
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
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            if (_context.Products == null)
            {
                return _context.Products != null ?
                       View(await _context.Products.ToListAsync()) :
                       Problem("Entity set 'ApplicationDbContext.Products'  is null.");

            }

            var products = await _context.Products.ToListAsync();

            if (products != null)
            {
                // Calculate total quantity, total cost, total sale price, profit, and markup
                var totalQuantity = products.Sum(item => item.Quantity);
                var totalCost = products.Sum(item => item.TotalPrice * item.Quantity);
                var totalSalePrice = totalCost * 1.33;

                var profit = totalSalePrice - totalCost;

                ViewBag.TotalQuantity = totalQuantity;
                ViewBag.TotalCost = totalCost.ToString("N2");
                ViewBag.TotalSale = totalSalePrice.ToString("N2");
                ViewBag.Profit = profit.ToString("N2");
            }

            return View(products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }
   


        [Authorize]
        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            return View(products);
        }

        [Authorize]
        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Name,Quantity,Paid,Status,TotalPrice")] Products products)
        {
            var existingProduct = await _context.Products.FindAsync(id);

            // If the product with the specified id is not found, existingProduct will be null
            if (existingProduct == null)
            {
                return NotFound(); // Or handle the situation as needed
            }

            if (ModelState.IsValid)
            {
                try
                {
                    existingProduct.Name = products.Name;
                    existingProduct.Quantity = products.Quantity;

                    _context.Update(existingProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.Id))
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
            return View(products);
        }

        // GET: Products/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }
            var products = await _context.Products.FindAsync(id);
            if (products != null)
            {
                _context.Products.Remove(products);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
          return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
