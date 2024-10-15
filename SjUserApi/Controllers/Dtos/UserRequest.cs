namespace SjUserApi.Controllers.Dtos;

public class UserRequest
{
    public required string Name { get; init; }
    public required string Role { get; init; }
}