using Going.Plaid;
using Going.Plaid.Link;
using Going.Plaid.Entity;
using Going.Plaid.Item;

using AssetTrackerWebAPI.Services;
using System.Security.Claims;


namespace AssetTrackerWebAPI.Services
{
    public class plaidService
    {
        private readonly PlaidClient _plaidClient;
        private readonly ProfileService _profileService;

        public plaidService(PlaidClient plaidClient, ProfileService profileService)
        {
            _plaidClient = plaidClient;
            _profileService = profileService;
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
        public async Task<string> ExchangeToken(string publicToken)
        {
            var response = await _plaidClient.ItemPublicTokenExchangeAsync(
                new ItemPublicTokenExchangeRequest
                {
                    PublicToken = publicToken
                });

            if (response.Error != null)
                throw new Exception($"Plaid error: {response.Error.ErrorMessage}");

            return response.AccessToken;
        }
    }
}