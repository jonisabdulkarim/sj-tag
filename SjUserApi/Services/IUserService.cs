using SjUserApi.Controllers.Dtos;

namespace SjUserApi.Services;

public interface IUserService
{
    public Task<UserResponse> CreateUserAsync(UserRequest request);
    public Task<UserResponse> UpdateUserAsync(Guid userId, UserRequest request);
    public Task<List<UserResponse>> ListUsersAsync();
    public Task<UserResponse> GetUserAsync(Guid userId);
    public Task DeleteUserAsync(Guid userId);
}