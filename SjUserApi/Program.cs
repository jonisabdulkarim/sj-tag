using Microsoft.Azure.Cosmos;
using SjUserApi.Configuration;
using SjUserApi.Middleware;
using SjUserApi.Repositories;
using SjUserApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile("secrets.json", optional: false, reloadOnChange: true);

var cosmosConfiguration = builder.Configuration.GetSection(CosmosSettings.SectionName);
builder.Services.Configure<CosmosSettings>(cosmosConfiguration);

var cosmosSettings = cosmosConfiguration.Get<CosmosSettings>();
var cosmosClient = new CosmosClient(cosmosSettings?.ConnectionString);
builder.Services.AddSingleton(cosmosClient);

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();