namespace SjUserApi.Configuration;

public class CosmosSettings
{
    public const string SectionName = "CosmosDb";
    public required string Database { get; set; }
    public required string Container { get; set; }
    public required string ConnectionString { get; set; }
}