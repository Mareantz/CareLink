using Domain.Entities;

namespace Domain.Repositories
{
	public interface IProductRepository
	{
		Task<Product> GetProductByIdAsync(Guid id);
		Task<IEnumerable<Product>> GetProductsAsync();
		Task<Guid> AddProductAsync(Product product);
		Task UpdateProductAsync(Product product);
		Task DeleteProductAsync(Guid id);
	}
}
