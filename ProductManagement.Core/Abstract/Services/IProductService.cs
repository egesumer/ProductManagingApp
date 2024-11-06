using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IProductService
{
    Task<bool> UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(Guid productId);
    Task<Product> GetProductByIdAsync(Guid productId);
    Task<IEnumerable<Product>> GetAllProductsAsync(bool includeDeleted = false);
    Task<Product> AddProductAsync(ProductCreateDto productDto);

}
