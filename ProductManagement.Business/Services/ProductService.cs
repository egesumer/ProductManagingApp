using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    // Yeni bir ürün ekleme
    public async Task<Product> AddProductAsync(ProductCreateDto productDto)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = productDto.Name,
            ProductCode = productDto.ProductCode,
            Price = productDto.Price,
            CreatedDate = DateTime.Now,
            ProductImage = productDto.ProductImage,
            IsDeleted = false
        };

        _productRepository.Add(product);
        return product;
    }

    // Ürün güncelleme
    public async Task<bool> UpdateProductAsync(Product product)
    {
        return _productRepository.Update(product);
    }

    // Ürün silme (soft delete)
    public async Task<bool> DeleteProductAsync(Guid productId)
    {
        var product = _productRepository.GetByID(productId);
        if (product == null)
        {
            throw new Exception("Product not found.");
        }
        product.IsDeleted = true;
        return _productRepository.Update(product);
    }

    // Belirli bir ürünü ID ile getirme
    public async Task<Product> GetProductByIdAsync(Guid productId)
    {
        return _productRepository.GetByID(productId);
    }

    // Tüm ürünleri getirme
    public async Task<IEnumerable<Product>> GetAllProductsAsync(bool includeDeleted = false)
    {
        return _productRepository.GetAll(includeDeleted);
    }

    public Task<Product> GetProductByIdAsync(Guid productId, bool includeDeleted = false)
    {
        throw new NotImplementedException();
    }
}
