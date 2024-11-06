using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class AuthTestController : ControllerBase
{
    [HttpGet]
    [Authorize]
    public IActionResult ProtectedEndpoint()
    {
        return Ok("Uygulama erişimine sahipsiniz.");
    }
}
