using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Contracts;

namespace ShopOnline.Api.Repositories
{
  public class ProductRepository : IProductRepository
  {
    private readonly ShopOnlineDataDbContext _context;

    public ProductRepository(ShopOnlineDataDbContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<ProductCategory>> GetCategories()
    {
      var categories = await _context.ProductCategories.ToListAsync();
      return categories;
    }

    public async Task<ProductCategory?> GetCategory(int id)
    {
      var category = await _context.ProductCategories.SingleOrDefaultAsync(c => c.Id == id);
      return category;
    }

    public async Task<Product?> GetProduct(int id)
    {
      var product = await _context.Products
                          .Include(p => p.ProductCategory)
                          .SingleOrDefaultAsync(p => p.Id == id);
      return product;
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
      var products = await _context.Products
                          .Include(p => p.ProductCategory)
                          .ToListAsync();
      return products;
    }

    public async Task<IEnumerable<Product>> GetProductsByCategory(int categoryId)
    {
      var products = await _context.Products
                          .Include(p => p.ProductCategory)
                          .Where(p => p.CategoryId == categoryId)
                          .ToListAsync();
      return products;
    }
  }
}
