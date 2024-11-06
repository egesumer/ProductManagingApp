using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Management.Core.Abstract;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // Yeni bir kullanıcı oluşturma
    public async Task<User> CreateUserAsync(CreateUserDto userDto)
    {
        var user = new User
        {
            Username = userDto.Username,
            Password = HashPassword(userDto.Password),
            UserType = userDto.UserType,
            IsDeleted = false // Yeni kullanıcı oluştururken IsDeleted varsayılan olarak false olmalı
        };

        bool created = _userRepository.Add(user);
        if (!created)
        {
            throw new Exception("User creation failed");
        }

        return user;
    }

    // Kullanıcı güncelleme
    public async Task<bool> UpdateUserAsync(Guid userId, UpdateUserDto userDto)
    {
        var user = _userRepository.GetByID(userId, includeDeleted: false);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        user.Username = userDto.Username;
        user.Password = HashPassword(userDto.Password);
        user.UserType = userDto.UserType;

        return _userRepository.Update(user);
    }

    // Kullanıcı soft delete
    public async Task<bool> SoftDeleteUserAsync(Guid userId)
    {
        var user = _userRepository.GetByID(userId, includeDeleted: false);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        return _userRepository.Delete(user);
    }

    // Tüm kullanıcıları getirme
    public async Task<IEnumerable<User>> GetAllUsersAsync(bool includeDeleted = false)
    {
        return _userRepository.GetAll(includeDeleted);
    }

    // Belirli bir kullanıcıyı ID ile getirme
    public async Task<User> GetUserByIdAsync(Guid userId, bool includeDeleted = false)
    {
        var user = _userRepository.GetByID(userId, includeDeleted);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        return user;
    }


    // Soft delete yapılmış bir kullanıcıyı tekrar aktif hale getirme
    public async Task<bool> ReactivateUserAsync(Guid userId)
    {
        var user = _userRepository.GetByID(userId, includeDeleted: true);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        if (!user.IsDeleted)
        {
            throw new InvalidOperationException("User is already active.");
        }

        user.IsDeleted = false;
        return _userRepository.Update(user);
    }

    // Yöneticileri listeleme
    public async Task<IEnumerable<User>> GetAllAdminsAsync()
    {
        return await _userRepository.GetUsersByTypeAsync(UserType.Admin);
    }

    // Müşterileri listeleme
    public async Task<IEnumerable<User>> GetAllCustomersAsync()
    {
        return await _userRepository.GetUsersByTypeAsync(UserType.Customer);
    }

    // Şifreyi hash'lemek için fonksiyon
    private string HashPassword(string password)
    {
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
    
}
