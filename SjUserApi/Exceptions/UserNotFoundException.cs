namespace SjUserApi.Exceptions;

public class UserNotFoundException(Guid id) : Exception($"User not found with id: {id}");