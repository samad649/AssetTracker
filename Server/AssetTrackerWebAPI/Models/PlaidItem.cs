public class PlaidItem
{
    [DynamoDBHashKey]
    public string UserId { get; set; } = string.Empty;      
    [DynamoDBRangeKey]
    public string ItemId { get; set; } = string.Empty;      
    public string AccessToken { get; set; } = string.Empty;
    public string CreatedAt { get; set; } = DateTime.UtcNow.ToString("o");
}