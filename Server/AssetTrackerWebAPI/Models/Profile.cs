using Amazon.DynamoDBv2.DataModel;

[DynamoDBTable("Profiles")]
public class Profile
{
    [DynamoDBHashKey]
    public string? profileId { get; set; }
    public List<Account>? accounts { get; set; }
    public string? firstName { get; set; }
    public string? lastName { get; set; }
    public string? email { get; set; }
}