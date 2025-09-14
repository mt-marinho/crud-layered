using Domain.Entities;
using Domain.Repositories;

namespace Application.Services
{
    public class ProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<int> CreateAsync(string name, decimal price)
        {
            var product = new Product(name, price);
            await _repo.AddAsync(product);
            return product.Id;
        }

        public Task<IReadOnlyList<Product>> ListAsync()
        {
            return _repo.ListAsync();
        }

        public Task<Product> GetAsync(int id)
        {
            return _repo.GetByIdAsync(id);
        }

        public async Task UpdateAsync(int id, string name, decimal price)
        {
            var p = await _repo.GetByIdAsync(id) ?? throw new ArgumentException("Produto não encontrado");
            p.UpdatePrice(name, price);
            await _repo.UpdateAsync(p);
        }

        public Task DeleteAsync(int id)
        {
            return _repo.DeleteAsync(id);
        }
    }
}
