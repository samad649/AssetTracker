using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
namespace AssetTrackerWebAPI.Services
{
    public class UserService{
        private readonly IDynamoDBContext _dynamoDBContext;

        public UserService(IDynamoDBContext dynamoDBContext)
        {
            _dynamoDBContext = dynamoDBContext;
        }
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var conditions = new List<ScanCondition>();
            var allUsers = await _dynamoDBContext.ScanAsync<User>(conditions).GetRemainingAsync();
            return allUsers;
        }
        public async Task<User> GetUser(string userId)
        {
            return await _dynamoDBContext.LoadAsync<User>(userId);
        }
    }
}