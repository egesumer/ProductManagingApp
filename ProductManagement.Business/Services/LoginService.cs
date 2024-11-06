
using System.Security.Cryptography;
using System.Text;
using Management.Business.Services;
using Management.Core.Abstract;

public class LoginService : ILoginService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService jwtService;

    public LoginService(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        this.jwtService = jwtService;
    }

    // Kullanıcıyı doğrulayan metot
    public async Task<string> LoginAsync(string username, string password)
    {
        var hashedPassword = PasswordHasher.HashPassword(password); // Kullanıcının girdiği şifre hashlenir.
        var user = await _userRepository.GetUserByUsernameAndPasswordAsync(username, hashedPassword); // Hashlenmiş şifre veritabanındaki hash ile karşılaştırılır.

        if (user == null)
        {
            throw new UnauthorizedAccessException("Kullanıcı adı veya şifre hatalı.");
        }

        return jwtService.GenerateToken(user);
    }
}