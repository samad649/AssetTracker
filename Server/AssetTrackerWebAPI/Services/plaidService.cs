using Going.Plaid;
using Going.Plaid.Link;
using Going.Plaid.Entity;
using Going.Plaid.Item;
using Going.Plaid.Accounts;
using Going.Plaid.Transactions;
using Amazon.DynamoDBv2.DataModel;

namespace AssetTrackerWebAPI.Services
{
    public class plaidService
    {
        private readonly IDynamoDBContext _dynamoDBContext;
        private readonly PlaidClient _plaidClient;
        private readonly plaidItemService _plaidItemService;

        public plaidService(PlaidClient plaidClient, IDynamoDBContext dynamoDBContext, plaidItemService plaidItemService)
        {
            _plaidClient = plaidClient;
            _dynamoDBContext = dynamoDBContext;
            _plaidItemService = plaidItemService;
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
        // Store transaction data for all accounts linked to a Plaid item
        public async Task storeTransactionData(string accessToken, string itemId, string profileId)
        {
            List<Transaction> transactions = new List<Transaction>();
            var removedList = new List<(string AccountId, string TransactionId)>();

            bool transactionsRemaining = true;
            string? cursor = await _plaidItemService.GetCursor(profileId, itemId); 

            while (transactionsRemaining)
            {
            var response = await _plaidClient.TransactionsSyncAsync(new TransactionsSyncRequest
                {
                    AccessToken = accessToken,
                    Cursor = cursor,
                    Count = 500,
                });
            if (response.Error != null){throw new Exception($"Plaid error: {response.Error.ErrorMessage}");}
            Console.WriteLine(response);
            transactions.AddRange(response.Added.Concat(response.Modified).Select(MapTransaction));
            removedList.AddRange(response.Removed.Select(r => (r.AccountId!, r.TransactionId!)));

            cursor = response.NextCursor;
            transactionsRemaining = response.HasMore;
            }
            await _plaidItemService.UpdateCursor(profileId, itemId, cursor);
            // Save transactions to DynamoDB
            foreach (var transaction in transactions)
            {
                await _dynamoDBContext.SaveAsync(transaction);
            }
            // Remove deleted transactions from DynamoDB
            foreach (var (accountId, transactionId) in removedList)
            {
                await _dynamoDBContext.DeleteAsync<Transaction>(accountId, transactionId);
            }

        }
        private Transaction MapTransaction(Going.Plaid.Entity.Transaction t)
        {
            return new Transaction
            {
                accountId = t.AccountId!,
                transactionId = t.TransactionId!,
                name = string.IsNullOrWhiteSpace(t.Name) ? null : t.Name,
                amount = t.Amount.HasValue ? (double)Math.Round(t.Amount.Value, 2) : null,
                ISOCurrencyCode = string.IsNullOrWhiteSpace(t.IsoCurrencyCode) ? null : t.IsoCurrencyCode,
                merchantName = string.IsNullOrWhiteSpace(t.MerchantName) ? null : t.MerchantName,
                merchantEntityId = string.IsNullOrWhiteSpace(t.MerchantEntityId) ? null : t.MerchantEntityId,
                logoUrl = string.IsNullOrWhiteSpace(t.LogoUrl) ? null : t.LogoUrl,
                date = t.Date?.ToDateTime(TimeOnly.MinValue),
                authorizedDate = t.AuthorizedDate?.ToDateTime(TimeOnly.MinValue),
                pending = t.Pending,
                pendingTransactionId = string.IsNullOrWhiteSpace(t.PendingTransactionId) ? null : t.PendingTransactionId,
                categoryPrimary = string.IsNullOrWhiteSpace(t.PersonalFinanceCategory?.Primary) ? null : t.PersonalFinanceCategory?.Primary,
                categoryDetailed = string.IsNullOrWhiteSpace(t.PersonalFinanceCategory?.Detailed) ? null : t.PersonalFinanceCategory?.Detailed,
                categoryConfidence = string.IsNullOrWhiteSpace(t.PersonalFinanceCategory?.ConfidenceLevel) ? null : t.PersonalFinanceCategory?.ConfidenceLevel,
                categoryIconUrl = string.IsNullOrWhiteSpace(t.PersonalFinanceCategoryIconUrl) ? null : t.PersonalFinanceCategoryIconUrl,
                paymentChannel = t.PaymentChannel?.ToString()
            };
        }
     }
}
