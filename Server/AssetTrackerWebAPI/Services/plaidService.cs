using Going.Plaid;
using Going.Plaid.Link;
using Going.Plaid.Entity;

namespace AssetTrackerWebAPI.Services
{
    public class PlaidService
    {
        private readonly PlaidClient _plaidClient;

        public PlaidService(PlaidClient plaidClient)
        {
            _plaidClient = plaidClient;
        }

        public async Task<string> CreateLinkToken(string userId)
        {
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
            var response = await _plaidClient.ItemPublicTokenExchangeAsync(
                new ItemPublicTokenExchangeRequest
                {
                    PublicToken = publicToken
                });

            if (response.Error != null)
                throw new Exception($"Plaid error: {response.Error.ErrorMessage}");

            var item = new PlaidItem
            {
                UserId = userId,
                AccessToken = response.AccessToken,
                ItemId = response.ItemId,
                CreatedAt = DateTime.UtcNow.ToString("o")
            };

            await _dbContext.SaveAsync(item);
            return item.ItemId;
        }
    }
}