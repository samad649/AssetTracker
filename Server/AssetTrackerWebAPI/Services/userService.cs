using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Mvc;
namespace AssetTrackerWebAPI.Services
{
    public class UserService{
       private readonly IDynamoDBContext _dynamoDBContext;

        public UserService(IDynamoDBContext dynamoDBContext)
        {
            _dynamoDBContext = dynamoDBContext;
        }
        public async Task<User> GetUser(string userId)
        {
            return await _dynamoDBContext.LoadAsync<User>(userId);
        }
        
        public async Task<string?> GetUserId(string username)
        {
            var user = await _dynamoDBContext.QueryAsync<User>(username, new QueryConfig
            {
                IndexName = "username-index"
            }).GetRemainingAsync();
            
            return user.FirstOrDefault()?.userId;
        }
        public async Task CreateUser(User user)
        {
             await _dynamoDBContext.SaveAsync(user);
        }
        
    }
}