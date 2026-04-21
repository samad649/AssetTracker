using Going.Plaid;
using Going.Plaid.Link;
using Going.Plaid.Entity;
using Going.Plaid.Item;
using Amazon.DynamoDBv2.DataModel;


namespace AssetTrackerWebAPI.Services
{
    public class PlaidService
    {
        private readonly PlaidClient _plaidClient;
        private readonly IDynamoDBContext _dynamoDb;

        public PlaidService(PlaidClient plaidClient, IDynamoDBContext dynamoDb)
        {
            _plaidClient = plaidClient;
            _dynamoDb = dynamoDb;
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
        public async Task ExchangeAndStoreToken(string publicToken, string userId)
        {
            Console.WriteLine($"=== ExchangeAndStoreToken called for userId: {userId} ===");
            var response = await _plaidClient.ItemPublicTokenExchangeAsync(
                new ItemPublicTokenExchangeRequest
                {
                    PublicToken = publicToken
                });
            Console.WriteLine($"=== 1Plaid response received for userId: {userId} ===");
            if (response.Error != null)
                throw new Exception($"Plaid error: {response.Error.ErrorMessage}");
            Console.WriteLine($"=== Plaid token exchange successful for userId: {userId}, itemId: {response.ItemId} ===");
            var item = new PlaidItem
            {
                userId = userId,
                accessToken = response.AccessToken,
                itemId = response.ItemId,
                createdAt = DateTime.UtcNow.ToString("o")
            };

            Console.WriteLine($"=== Exchanged public token for userId: {userId}, itemId: {item.itemId} ===");
            await _dynamoDb.SaveAsync(item);
            Console.WriteLine($"=== Stored Plaid item for userId: {userId}, itemId: {item.itemId} ===");
        }
    }
}