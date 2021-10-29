using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ApplicationDevelopmentCourseProject.Data;
using ApplicationDevelopmentCourseProject.Models;
using Microsoft.AspNetCore.Authorization;

namespace ApplicationDevelopmentCourseProject.Controllers
{
    public class ProductTagsController : Controller
    {
        private readonly ApplicationDevelopmentCourseProjectContext _context;

        public ProductTagsController(ApplicationDevelopmentCourseProjectContext context)
        {
            _context = context;
        }

        // GET: ProductTags
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductTag.ToListAsync());
        }

        // GET: ProductTags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productTag = await _context.ProductTag
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productTag == null)
            {
                return NotFound();
            }

            return View(productTag);
        }

        // GET: ProductTags/Create
        [Authorize(Roles = "Admin")]

        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductTags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Create([Bind("Id,TagName")] ProductTag productTag)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productTag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productTag);
        }

        // GET: ProductTags/Edit/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productTag = await _context.ProductTag.FindAsync(id);
            if (productTag == null)
            {
                return NotFound();
            }
            return View(productTag);
        }

        // POST: ProductTags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int id, [Bind("Id,TagName")] ProductTag productTag)
        {
            if (id != productTag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productTag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductTagExists(productTag.Id))
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
            return View(productTag);
        }

        // GET: ProductTags/Delete/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productTag = await _context.ProductTag
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productTag == null)
            {
                return NotFound();
            }

            return View(productTag);
        }

        // POST: ProductTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productTag = await _context.ProductTag.FindAsync(id);
            _context.ProductTag.Remove(productTag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]

        private bool ProductTagExists(int id)
        {
            return _context.ProductTag.Any(e => e.Id == id);
        }
    }
}
