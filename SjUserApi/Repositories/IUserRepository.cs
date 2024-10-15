namespace SjUserApi.Repositories;

public interface IUserRepository
{
    public Task<User> AddUserAsync(User user);
    public Task<User> GetUserAsync(Guid userId);
    public Task<IEnumerable<User>> GetUsersAsync();
    public Task<User> UpdateUserAsync(User user);
    public Task DeleteUserAsync(Guid userId);
}