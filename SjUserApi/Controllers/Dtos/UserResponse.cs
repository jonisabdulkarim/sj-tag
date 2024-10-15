namespace SjUserApi.Controllers.Dtos;

public class UserResponse(User user)
{
    public Guid Id { get; init; } = user.id;
    public string Name { get; init; } = user.Name;
    public string Role { get; init; } = user.Role;
}