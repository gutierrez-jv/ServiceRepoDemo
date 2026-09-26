using Microsoft.EntityFrameworkCore;
using ServiceRepoDemo.Data;
using ServiceRepoDemo.Models;

namespace ServiceRepoDemo.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync() =>
        await _context.Products.AsNoTracking().OrderBy(p => p.Description).ToListAsync();

    public async Task<Product?> GetByIdAsync(int id) =>
        await _context.Products.FindAsync(id);

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
    }

    public void Update(Product product) =>
        _context.Products.Update(product);

    public void Delete(Product product) =>
        _context.Entry(product).State = EntityState.Detached;

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
