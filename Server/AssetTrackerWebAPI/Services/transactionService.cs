using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace AssetTrackerWebAPI.Services
{
  public class transactionService{
        private readonly IDynamoDBContext _dynamoDBContext;
        private readonly accountService _accountService;

        public transactionService(IDynamoDBContext dynamoDBContext, accountService accountService)
        {
            _dynamoDBContext = dynamoDBContext;
            _accountService = accountService;
        }
        public async Task<List<Transaction>> GetTransactionsByProfile(string profileId)
        {
            var accounts = await _accountService.GetAllAccounts(profileId);
            
            var tasks = accounts.Select(account => GetTransactionsByAccount(account.accountId));
            var results = await Task.WhenAll(tasks);
            
            return results.SelectMany(t => t).ToList();
        }
        public async Task<List<Transaction>> GetTransactionsByAccount(string accountId)
        {
            var transactions = await _dynamoDBContext.QueryAsync<Transaction>(accountId).GetRemainingAsync();
            return transactions;
        }
    }  
}