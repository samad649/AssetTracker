using Amazon.DynamoDBv2.DataModel;

[DynamoDBTable("PlaidItems")]
public class PlaidItem
{
    [DynamoDBHashKey]
    public string userId { get; set; } = string.Empty;      
    [DynamoDBRangeKey]
    public string itemId { get; set; } = string.Empty;      
    public string accessToken { get; set; } = string.Empty;
    public string createdAt { get; set; } = DateTime.UtcNow.ToString("o");
}