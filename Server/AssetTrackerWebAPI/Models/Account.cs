using Amazon.DynamoDBv2.DataModel;

[DynamoDBTable("Accounts")]
public class Account
{
    [DynamoDBHashKey]
    public string? accountId { get; set; }
    public float? balance { get; set; }
    public string? institution { get; set; }
}