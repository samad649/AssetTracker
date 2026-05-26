using Going.Plaid;
using Going.Plaid.Link;
using Going.Plaid.Entity;
using Going.Plaid.Item;
using Going.Plaid.Accounts;
using Amazon.DynamoDBv2.DataModel;

namespace AssetTrackerWebAPI.Services
{
    public class plaidService
    {
        private readonly IDynamoDBContext _dynamoDBContext;
        private readonly PlaidClient _plaidClient;

        public plaidService(PlaidClient plaidClient, IDynamoDBContext dynamoDBContext)
        {
            _plaidClient = plaidClient;
            _dynamoDBContext = dynamoDBContext;
        }

        public async Task<string> CreateLinkToken(string userId)
        {
        Console.WriteLine($"=== CreateLinkToken called for userId: {userId} ===");

        var request = new LinkTokenCreateRequest
        {
            User = new LinkTokenCreateRequestUser
            {
                ClientUserId = userId
            },
            ClientName = "AssetTracker",
            Products = new List<Products> { Products.Auth, Products.Transactions },
            CountryCodes = new List<CountryCode> { CountryCode.Us },
            Language = Language.English,
        };

        var response = await _plaidClient.LinkTokenCreateAsync(request);

        if (response.Error != null)
            throw new Exception($"Plaid error [{response.Error.ErrorCode}]: {response.Error.ErrorMessage}");

        if (string.IsNullOrEmpty(response.LinkToken))
            throw new Exception("Plaid returned no link token and no error — check your credentials in appsettings.json");

        return response.LinkToken;
        }
        public async Task<(string accessToken, string itemId)> ExchangeToken(string publicToken)
        {
            var response = await _plaidClient.ItemPublicTokenExchangeAsync(
                new ItemPublicTokenExchangeRequest
                {
                    PublicToken = publicToken
                });

            if (response.Error != null)
                throw new Exception($"Plaid error: {response.Error.ErrorMessage}");

            return (response.AccessToken, response.ItemId); 
        }
        public async Task storeAccountData(string accessToken, string itemId, string profileId, string institutionName)
        {
            var accountsResponse = await _plaidClient.AccountsGetAsync(new AccountsGetRequest
            {
                AccessToken = accessToken
            });

            if (accountsResponse.Error != null)
                throw new Exception($"Plaid error: {accountsResponse.Error.ErrorMessage}");

            var accounts = accountsResponse.Accounts;
            foreach(var account in accounts)
            {
            Console.WriteLine($"Storing account: {account.AccountId} - {account.Name} - {account.Balances.Current}");
            var newAccount = new Account
                {
                    profileId = profileId,
                    accountId = account.AccountId,
                    itemId = itemId,
                    name = account.Name,
                    mask = account.Mask ?? string.Empty,
                    type = account.Type.ToString(),
                    subtype = account.Subtype?.ToString() ?? string.Empty,
                    currentBalance = account.Balances.Current,
                    availableBalance = account.Balances.Available,
                    lastUpdated = DateTime.UtcNow.ToString("o"),
                    institutionName = institutionName
                };
                    await _dynamoDBContext.SaveAsync(newAccount);
            }

        }
    }
}