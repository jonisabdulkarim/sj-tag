using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using SjUserApi.Configuration;
using SjUserApi.Repositories;

namespace SjUserApi.Test;

public class UserRepositoryTest : IAsyncLifetime
{
    private readonly UserRepository _repository;

    public UserRepositoryTest()
    {
        // create settings
        var settings = new CosmosSettings
        {
            ConnectionString = "",
            Database = "SjUserApi",
            Container = "User"
        };
        var cosmosSettings = Options.Create(settings);
        
        // create client and repository
        var cosmosClient = new CosmosClient(cosmosSettings.Value.ConnectionString);
        _repository = new UserRepository(cosmosSettings, cosmosClient);
    }

    public async Task InitializeAsync()
    {
        var newUser = new User
        {
            id = Guid.Empty,
            Name = "Jonis",
            Role = "Engineer"
        };
        
        await _repository.AddUserAsync(newUser);
    }

    public async Task DisposeAsync()
    {
        await _repository.DeleteUserAsync(Guid.Empty);
    }

    [Fact]
    public async void CreateThenDeleteUserTest()
    {
        var newUser = new User
        {
            id = Guid.Parse("a236240b-68a3-4fbb-846f-eb20de914224"),
            Name = "Jonis",
            Role = "Engineer"
        };
        
        var result = await _repository.AddUserAsync(newUser);
        
        Assert.NotNull(result);
        Assert.Equal("Jonis", result.Name);
        Assert.Equal("Engineer", result.Role);
        
        await _repository.DeleteUserAsync(newUser.id);
    }

    [Fact]
    public async void CreateThenGetUserTest()
    {
        var newUser = new User
        {
            id = Guid.Parse("a236240b-68a3-4fbb-846f-eb20de914224"),
            Name = "Jonis",
            Role = "Engineer"
        };
        
        await _repository.AddUserAsync(newUser);
        var result = await _repository.GetUserAsync(newUser.id);
        
        Assert.NotNull(result);
        Assert.Equal("Jonis", result.Name);
        Assert.Equal("Engineer", result.Role);
        
        await _repository.DeleteUserAsync(newUser.id);
    }

    [Fact]
    public async void UpdateUserTest()
    {
        var user = new User()
        {
            id = Guid.Empty,
            Name = "James",
            Role = "Emperor"
        };
        
        var result = await _repository.UpdateUserAsync(user);
        
        Assert.NotNull(result);
        Assert.Equal("James", result.Name);
        Assert.Equal("Emperor", result.Role);
    }

    [Fact]
    public async void GetUserTest()
    {
        var result = await _repository.GetUserAsync(Guid.Empty);
        
        Assert.NotNull(result);
        Assert.Equal("Jonis", result.Name);
        Assert.Equal("Engineer", result.Role);
    }

}