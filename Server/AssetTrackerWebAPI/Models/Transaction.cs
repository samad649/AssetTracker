using Amazon.DynamoDBv2.DataModel;

[DynamoDBTable("Transactions")]
public class Transaction
{
    [DynamoDBHashKey]
    public required string transactionId { get; set; }
    public required string accountId { get; set; }
    public string? vendor { get; set; }
    public float? amount { get; set; }
    public DateTime? date { get; set; }
}