using Amazon.DynamoDBv2.DataModel;
namespace AssetTrackerWebAPI.Services
{
    public class ProfileService
    {
        private readonly IDynamoDBContext _dynamoDBContext;

        public ProfileService(IDynamoDBContext dynamoDBContext)
        {
            _dynamoDBContext = dynamoDBContext;
        }
        public Profile CreateProfile()
        {
            var account1 = new Account();
            var account2 = new Account();
            account1.accountId = "account1";
            account1.balance = 1000.00f;
            account2.accountId = "account2";
            account2.balance = 2000.00f;
            account1.institution = "Chase";
            account2.institution = "SoFi";
            List<Account> accounts = new List<Account>();
            accounts.Add(account1);
            accounts.Add(account2);
            Profile profile = new Profile();
            profile.firstName = "Samee";
            profile.lastName = "Prasla";
            profile.accounts = accounts;
            return profile;
        }
        public async Task<Account> CreateAccountAsync(Account account)
        {
            await _dynamoDBContext.SaveAsync(account);
            return account;
        }
    }
}