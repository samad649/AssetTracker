using Amazon.DynamoDBv2.DataModel;

[DynamoDBTable("Accounts")]
public class Account
{
    [DynamoDBHashKey]
    public required string profileId { get; set; }
    public required string accountId { get; set; }
    public string? type { get; set; }
    public float? balance { get; set; }
    public string? institution { get; set; }
}