using Microsoft.EntityFrameworkCore;
using ProductManagement.Data;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    private readonly AppDbContext db;
    public ProductRepository(AppDbContext db) : base(db)
    {
        this.db = db;
    }
}