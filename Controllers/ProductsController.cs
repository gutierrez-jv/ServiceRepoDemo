using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceRepoDemo.Data;
using ServiceRepoDemo.Models;
using ServiceRepoDemo.Services;

namespace ServiceRepoDemo.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly AppDbContext _context;

    public ProductsController(IProductService productService, AppDbContext context)
    {
        _productService = productService;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _context.Products.OrderBy(p => p.Name).ToListAsync();
        return View(products);
    }

    public IActionResult Create() => View(new Product());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product)
    {
        if (!ModelState.IsValid) return View(product);

        var result = await _productService.CreateAsync(product);
        if (!result.Success)
        {
            TempData["Message"] = result.Error;
            return RedirectToAction(nameof(Create));
        }

        TempData["Message"] = "Product created.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        return product is null ? NotFound() : View(product);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Product product)
    {
        if (id != product.Id) return BadRequest();
        if (!ModelState.IsValid) return View(product);

        var result = await _productService.UpdateAsync(product);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Error!);
            return View(product);
        }

        TempData["Message"] = "Product updated.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        return product is null ? NotFound() : View(product);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product is null) return NotFound();

        if (product.Stock >= 0)
        {
            TempData["Message"] = "Cannot delete a product that still has stock.";
            return RedirectToAction(nameof(Index));
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        TempData["Message"] = "Product deleted.";
        return RedirectToAction(nameof(Index));
    }
}
