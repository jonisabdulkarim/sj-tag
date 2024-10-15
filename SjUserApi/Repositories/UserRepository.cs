using System.Net;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using SjUserApi.Configuration;
using SjUserApi.Exceptions;

namespace SjUserApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly Container _container;

    public UserRepository(IOptions<CosmosSettings> settings, CosmosClient cosmosClient)
    {
        var cosmosSettings = settings.Value;
        _container = cosmosClient.GetContainer(cosmosSettings.Database, cosmosSettings.Container);
    }

    public async Task<User> AddUserAsync(User user)
    {
        return await _container.CreateItemAsync(user);
    }

    public async Task<User> GetUserAsync(Guid userId)
    {
        try
        {
            return await _container.ReadItemAsync<User>(userId.ToString(), new PartitionKey(userId.ToString()));
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            throw new UserNotFoundException(userId);
        }
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        var query = _container.GetItemQueryIterator<User>();
        var results = new List<User>();

        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response.ToList());
        }

        return results;
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        try
        {
            return await _container.ReplaceItemAsync(user, user.id.ToString(), new PartitionKey(user.id.ToString()));
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            throw new UserNotFoundException(user.id);
        }
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        try
        {
            await _container.DeleteItemAsync<User>(userId.ToString(), new PartitionKey(userId.ToString()));
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            throw new UserNotFoundException(userId);
        }
    }
}