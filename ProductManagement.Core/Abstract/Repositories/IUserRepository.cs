public interface IUserRepository:IRepository<User>
{
    Task<User> GetUserByUsernameAndPasswordAsync(string username, string password);
    Task<IEnumerable<User>> GetUsersByTypeAsync(UserType userType);
}