using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Common.Repositories; 
using Members.Entities;

namespace JwtUsers.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class SubscriptionController : ControllerBase
{
    private readonly IRepository<User> _userRepository;

    public SubscriptionController(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Subscribe()
    {
        try
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrWhiteSpace(email))
                return Unauthorized("Email not found in token.");

            var user = await _userRepository.GetAsync(u => u.Email == email);
            if (user == null)
                return NotFound("User not found.");

            user.NewSubscription = true;
            await _userRepository.UpdateAsync(user);

            return Ok("Subscription updated successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}
