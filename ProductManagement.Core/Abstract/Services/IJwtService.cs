
namespace Management.Core.Abstract
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
