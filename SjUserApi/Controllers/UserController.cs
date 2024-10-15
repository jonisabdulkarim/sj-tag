using Microsoft.AspNetCore.Mvc;
using SjUserApi.Controllers.Dtos;
using SjUserApi.Services;

namespace SjUserApi.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateUser(UserRequest request)
    {
        var result = await userService.CreateUserAsync(request);
        return Created($"{result.Id}", result);
    }
    
    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetUser(Guid userId)
    {
        return Ok(await userService.GetUserAsync(userId));
    }
    
    [HttpGet]
    public async Task<IActionResult> ListUsers()
    {
        return Ok(await userService.ListUsersAsync());
    }
    
    [HttpPut("{userId:guid}")]
    public async Task<IActionResult> UpdateUser(Guid userId, UserRequest request)
    {
        return Ok(await userService.UpdateUserAsync(userId, request));
    }
    
    [HttpDelete("{userId:guid}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        await userService.DeleteUserAsync(userId);
        return Ok();
    }
    
}