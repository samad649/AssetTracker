using Amazon.DynamoDBv2.DataModel;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims; 

namespace AssetTrackerWebAPI.Services
{
    public class ProfileService
    {
        private readonly IDynamoDBContext _dynamoDBContext;

        public ProfileService(IDynamoDBContext dynamoDBContext)
        {
            _dynamoDBContext = dynamoDBContext;
        }
        public async Task<IEnumerable<Profile>> GetAllProfiles()
        {
            var conditions = new List<ScanCondition>();
            var allProfiles = await _dynamoDBContext.ScanAsync<Profile>(conditions).GetRemainingAsync();
            return allProfiles;
        }
        public async Task<Profile> GetProfile(string profileId)
        {
            return await _dynamoDBContext.LoadAsync<Profile>(profileId);
        }
        public async Task<IEnumerable<Account>> GetProfileAccounts(string profileId)
        {
            return await _dynamoDBContext.QueryAsync<Account>(profileId).GetRemainingAsync();
        }
        public async Task<IEnumerable<Transaction>> GetAccountTransactions(string accountId)
        {
            return await _dynamoDBContext.QueryAsync<Transaction>(accountId).GetRemainingAsync();
        }
}
}