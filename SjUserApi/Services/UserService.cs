using SjUserApi.Controllers.Dtos;
using SjUserApi.Repositories;

namespace SjUserApi.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<UserResponse> CreateUserAsync(UserRequest request)
    {
        var user = new User
        {
            id = Guid.NewGuid(),
            Name = request.Name,
            Role = request.Role
        };
        var createdUser = await userRepository.AddUserAsync(user);

        return new UserResponse(createdUser);
    }

    public async Task<UserResponse> UpdateUserAsync(Guid userId, UserRequest request)
    {
        var oldUser = await userRepository.GetUserAsync(userId);
        
        var newUser = new User
        {
            id = oldUser.id,
            Name = request.Name,
            Role = request.Role
        };
        var updatedUser = await userRepository.UpdateUserAsync(newUser);

        return new UserResponse(updatedUser);
    }

    public async Task<List<UserResponse>> ListUsersAsync()
    {
        var listOfUsers = await userRepository.GetUsersAsync();
        
        return listOfUsers.Select(user => new UserResponse(user)).ToList();
    }

    public async Task<UserResponse> GetUserAsync(Guid userId)
    {
        var user = await userRepository.GetUserAsync(userId);
        return new UserResponse(user);
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        await userRepository.DeleteUserAsync(userId);
    }
}