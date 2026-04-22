using Amazon.DynamoDBv2.DataModel;

[DynamoDBTable("Accounts")]
public class Account
{
    [DynamoDBHashKey]
    public required string profileId { get; set; }
    [DynamoDBRangeKey]
    public required string accountId { get; set; }
    [DynamoDBGlobalSecondaryIndexHashKey("itemId-index")]
    public string itemId { get; set; } = string.Empty;
    public string name { get; set; } = string.Empty;
    public string mask { get; set; } = string.Empty;
    public string type { get; set; } = string.Empty;
    public string subtype { get; set; } = string.Empty;


}