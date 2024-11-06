
using Microsoft.EntityFrameworkCore;
using ProductManagement.Data;

public class UserRepository : GenericRepository<User>,IUserRepository
{
    private readonly AppDbContext db;
    public UserRepository(AppDbContext db) : base(db)
    {
        this.db = db;
    }

    public async Task<User> GetUserByUsernameAndPasswordAsync(string username, string password)
    {
        return await db.Users
            .Where(u => u.Username == username && u.Password == password && !u.IsDeleted)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<User>> GetUsersByTypeAsync(UserType userType)
    {
        return await db.Users
                       .Where(u => u.UserType == userType && !u.IsDeleted) // Sadece belirtilen UserType ve aktif kullanıcılar
                       .ToListAsync();
    }
}