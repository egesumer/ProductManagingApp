public interface IUserService
{
    Task<User> CreateUserAsync(CreateUserDto userDto);
    Task<bool> UpdateUserAsync(Guid userId, UpdateUserDto userDto);
    Task<bool> SoftDeleteUserAsync(Guid userId);
    Task<IEnumerable<User>> GetAllUsersAsync(bool includeDeleted = false);
    Task<User> GetUserByIdAsync(Guid userId, bool includeDeleted = false);
    Task<bool> ReactivateUserAsync(Guid userId);
    Task<IEnumerable<User>> GetAllAdminsAsync();
    Task<IEnumerable<User>> GetAllCustomersAsync();
}
