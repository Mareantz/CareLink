using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistance;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
	{
        private readonly ApplicationDbContext context;
		public ProductRepository(ApplicationDbContext context)
		{
			this.context = context;
		}

		public async Task<Product> GetProductByIdAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<Product>> GetProductsAsync()
		{
			throw new NotImplementedException();
		}

		public async Task<Guid> AddProductAsync(Product product)
		{
			await context.Products.AddAsync(product);
			await context.SaveChangesAsync();
			return product.Id;
		}

		public async Task UpdateProductAsync(Product product)
		{
			throw new NotImplementedException();
		}

		public async Task DeleteProductAsync(Guid id)
		{
			throw new NotImplementedException();
		}
	}
}
