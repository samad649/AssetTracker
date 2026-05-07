using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
namespace AssetTrackerWebAPI.Services
{
    public class accountService{
        private readonly IDynamoDBContext _dynamoDBContext;

        public accountService(IDynamoDBContext dynamoDBContext)
        {
            _dynamoDBContext = dynamoDBContext;
        }
        public async Task<IEnumerable<Account>> GetAllAccounts(string profileId)
        {
          var accounts = await _dynamoDBContext.QueryAsync<Account>(profileId).GetRemainingAsync();
          return accounts;
        }
        public async Task<Account> GetAccount(string profileId, string accountId)
        {
            return await _dynamoDBContext.LoadAsync<Account>(profileId, accountId);
        }
        public async Task<List<Account>> GetAccountsByItemId(string itemId)
        {
            return await _dynamoDBContext
                .QueryAsync<Account>(itemId, new QueryConfig
                {
                    IndexName = "itemId-index"
                })
                .GetRemainingAsync();
        }
    }
}