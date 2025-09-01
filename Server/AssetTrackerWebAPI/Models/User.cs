using Amazon.DynamoDBv2.DataModel;

namespace AssetTrackerWebAPI.Models
{
    public class User
    {
        [DynamoDBHashKey("user_id")]
        public Guid UserId { get; set; }
        [DynamoDBProperty("first_name")]
        public required string FirstName { get; set; }
        [DynamoDBProperty("last_name")]
        public required string LastName { get; set; }
        [DynamoDBProperty("cognito_user_id")]
        public required string CognitoUserId { get; set; }
        [DynamoDBProperty("email")]
        public required string Email { get; set; }
        //[DynamoDBProperty("plaid_connections")]
        //public required List<PlaidConnection> PlaidConnections { get; set; }

        public User()
        {
            UserId = Guid.NewGuid();
            //PlaidConnections = new List<PlaidConnection>();
        }

        public User(string firstName, string lastName, string cognitoUserId, string email): this()
        {
            
            FirstName = firstName;
            LastName = lastName;
            CognitoUserId = cognitoUserId;
            Email = email;
            
        }
    }
    


}
