using System.ComponentModel.Design.Serialization;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Data {
  public class ProductRepository : IProductRepository {
    private readonly StoreContext _context;
    public ProductRepository(StoreContext context) {
      _context = context;
    }
    public void CreateProduct(Product product) {
      _context.Products.Add(product);
    }

    public void DeleteProduct(Product product) {
      _context.Products.Remove(product);
    }

    public async Task<IReadOnlyList<string>> GetBrandsAsync() {
      return await _context.Products.Select(p => p.Brand).Distinct().ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id) {
      return await _context.Products.FindAsync(id);
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync() {
      return await _context.Products.ToListAsync();
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort) {
      var query = _context.Products.AsQueryable();
      if (!brand.IsNullOrEmpty()) {
        query = query.Where(p => p.Brand.Equals(brand));
      }
      if (!type.IsNullOrEmpty()) {
        query = query.Where(p => p.Type.Equals(type));
      }

      //switch expression (for default case we need to use _)
      query = sort switch {
        "priceAsc" => query.OrderBy(p => p.Price),
        "priceDsc" => query.OrderByDescending(p => p.Price),
        _ => query.OrderBy(p => p.Name)
      };

      return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<string>> GetTypesAsync() {
      return await _context.Products.Select(p => p.Type).Distinct().ToListAsync();
    }

    public bool ProductExists(int id) {
      return _context.Products.Any(p => p.Id == id);
    }

    public async Task<bool> SaveChangesAsync() {
      return await _context.SaveChangesAsync() > 0;
    }

    public void UpdateProduct(Product product) {
      _context.Entry(product).State = EntityState.Modified;
    }
  }
}
