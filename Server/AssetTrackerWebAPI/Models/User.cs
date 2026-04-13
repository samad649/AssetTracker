using Amazon.DynamoDBv2.DataModel;

[DynamoDBTable("Users")]
public class User
{
    [DynamoDBHashKey]
    public required string userId { get; set; }
    public string? profileId { get; set; }
    public string? email { get; set; }
    [DynamoDBGlobalSecondaryIndexHashKey("username-index")]
    public string? username { get; set; }
    public string? password { get; set; }

}