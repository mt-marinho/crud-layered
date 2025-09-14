using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _db;
        public ProductRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Product> GetByIdAsync(int id) => await _db.products.FindAsync(id);
        public async Task<IReadOnlyList<Product>> ListAsync() => await _db.products.AsNoTracking().ToListAsync();
        public async Task AddAsync(Product product)
        {
            _db.products.Add(product);
            await _db.SaveChangesAsync();
        }
        public async Task UpdateAsync(Product product)
        {
            _db.products.Update(product);
            await _db.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var p = await _db.products.FindAsync(id);
            if (p != null)
            {
                _db.products.Remove(p);
                await _db.SaveChangesAsync();
            }
        }
    }
}