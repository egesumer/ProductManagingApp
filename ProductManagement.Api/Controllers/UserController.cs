using Management.Api.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    // Tüm kullanıcıları getirme (Sadece Admin ve SuperAdmin erişebilir)
    [HttpGet("get-users")]
    [Authorize]
    [UserTypeAuthorize(UserType.Admin, UserType.Superadmin)]
    public async Task<IActionResult> GetAllUsers([FromQuery] bool includeDeleted = false)
    {
        var users = await _userService.GetAllUsersAsync(includeDeleted);
        return Ok(users);
    }

    // Kullanıcıyı ID ile getirme (Sadece Admin ve SuperAdmin erişebilir)
    [HttpGet("{id}/get-user-by-id")]
    [Authorize]
    [UserTypeAuthorize(UserType.Admin, UserType.Superadmin)]
    public async Task<IActionResult> GetUserById(Guid id, [FromQuery] bool includeDeleted = false)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id, includeDeleted);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // Yeni kullanıcı oluşturma (Sadece Admin ve SuperAdmin erişebilir)
    [HttpPost("create-user")]
    [Authorize]
    [UserTypeAuthorize(UserType.Admin, UserType.Superadmin)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
    {
        try
        {
            var user = await _userService.CreateUserAsync(userDto);
            return Ok(new { message = "User created successfully", user });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // Kullanıcı güncelleme (Customer kendi profilini güncelleyebilir, Admin ve SuperAdmin tüm kullanıcıları güncelleyebilir)
    [HttpPut("{id}/update-user")]
    [Authorize]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto userDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userTypeClaim = User.FindFirst("UserType")?.Value;

        // Müşteri kendi profilini güncelleyebilir, Admin ve SuperAdmin tüm kullanıcıları güncelleyebilir
        if (userTypeClaim == UserType.Customer.ToString() && id.ToString() != userId)
        {
            return Forbid("You can only update your own profile.");
        }

        // Eğer güncelleme isteğinde UserType alanı değiştiriliyorsa ve güncellemeyi yapan kullanıcı Admin değilse, işlem yasaklanır
        var existingUser = await _userService.GetUserByIdAsync(id);
        if (existingUser == null)
        {
            return NotFound(new { message = "User not found" });
        }

        // Eğer güncellenmek istenen UserType mevcut UserType'tan farklı ise ve kullanıcı Admin veya SuperAdmin değilse, işlem yasaklanır
        if (existingUser.UserType != userDto.UserType && userTypeClaim != UserType.Admin.ToString() && userTypeClaim != UserType.Superadmin.ToString())
        {
            return Forbid("You are not authorized to change the UserType.");
        }

        try
        {
            var updated = await _userService.UpdateUserAsync(id, userDto);
            if (updated)
                return Ok(new { message = "User updated successfully" });
            return BadRequest(new { message = "User update failed" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }


    // Kullanıcı soft delete (Sadece Admin ve SuperAdmin erişebilir)
    [HttpDelete("{id}/delete-user")]
    [Authorize]
    [UserTypeAuthorize(UserType.Admin, UserType.Superadmin)]
    public async Task<IActionResult> SoftDeleteUser(Guid id)
    {
        try
        {
            var deleted = await _userService.SoftDeleteUserAsync(id);
            if (deleted)
                return Ok(new { message = "User soft-deleted successfully" });
            return BadRequest(new { message = "User deletion failed" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // Soft delete yapılmış kullanıcıyı tekrar aktif etme (Sadece Admin ve SuperAdmin erişebilir)
    [HttpPut("{id}/reactivate-user")]
    [Authorize]
    [UserTypeAuthorize(UserType.Admin, UserType.Superadmin)]
    public async Task<IActionResult> ReactivateUser(Guid id)
    {
        try
        {
            var reactivated = await _userService.ReactivateUserAsync(id);
            if (reactivated)
                return Ok(new { message = "User reactivated successfully" });
            return BadRequest(new { message = "User reactivation failed" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // Yöneticileri listele (Sadece Admin ve SuperAdmin erişebilir)
    [HttpGet("get-admins")]
    [Authorize]
    [UserTypeAuthorize(UserType.Admin, UserType.Superadmin)]
    public async Task<IActionResult> GetAllAdmins()
    {
        var admins = await _userService.GetAllAdminsAsync();
        return Ok(admins);
    }

    // Müşterileri listele (Sadece Admin ve SuperAdmin erişebilir)
    [HttpGet("get-customers")]
    [Authorize]
    [UserTypeAuthorize(UserType.Admin, UserType.Superadmin)]
    public async Task<IActionResult> GetAllCustomers()
    {
        var customers = await _userService.GetAllCustomersAsync();
        return Ok(customers);
    }
}
