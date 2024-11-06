using Microsoft.EntityFrameworkCore;

namespace ProductManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; } 
        public DbSet<Product> Products { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

             modelBuilder.Entity<User>()
            .ToTable("User")
            .HasIndex(u => u.Username)
            .IsUnique(); 

            //  Superadmin kullanıcısını seed data olarak ekleme
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = Guid.Parse("d2f62e3e-4db4-4ae7-a9f7-96c5d90f77fc"), 
                    Username = "superadmin",
                    Password = PasswordHasher.HashPassword("superadmin"),
                    UserType = UserType.Superadmin
                }
            );

            modelBuilder.Entity<Product>()
            .HasIndex(p => p.ProductCode)
            .IsUnique();

            modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = Guid.Parse("ace77301-fc64-4da9-bb59-9657d3335b28"), 
                Name = "Elma",
                ProductCode = "ELMA",
                Price = 15,
                CreatedDate = DateTime.Now,
                ProductImage = "https://upload.wikimedia.org/wikipedia/commons/1/15/Red_Apple.jpg"
            },
            new Product
            {
                Id = Guid.Parse("2d8d45f6-80e0-4acc-9685-652da49bc67b"), 
                Name = "Muz",
                ProductCode = "MUZ",
                Price = 30,
                CreatedDate = DateTime.Now,
                ProductImage = "https://upload.wikimedia.org/wikipedia/commons/8/8a/Banana-Single.jpg"
            }
        );
        }
    }
}
