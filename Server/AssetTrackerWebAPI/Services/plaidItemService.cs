
using Amazon.DynamoDBv2.DataModel;
namespace AssetTrackerWebAPI.Services
{
    public class plaidItemService{
        private readonly IDynamoDBContext _dynamoDBContext;
        
        public plaidItemService(IDynamoDBContext dynamoDBContext)
        {
            _dynamoDBContext = dynamoDBContext;
        }
        public async Task<IEnumerable<PlaidItem>> GetPlaidItems(string profileId)
        {
           var plaidItems = await _dynamoDBContext.QueryAsync<PlaidItem>(profileId).GetRemainingAsync();
           if (plaidItems == null || !plaidItems.Any())
           {
            Console.WriteLine($"No Plaid items found for profileId: {profileId}");
               return Enumerable.Empty<PlaidItem>();
           }
           return plaidItems;
        }
        public async Task<IEnumerable<PlaidItem>> GetItemsByProfile(string profileId)
        {
            var plaidItems = await _dynamoDBContext.QueryAsync<PlaidItem>(profileId).GetRemainingAsync();
            if (plaidItems == null || !plaidItems.Any())
            {
                Console.WriteLine($"No Plaid items found for profileId: {profileId}");
                return Enumerable.Empty<PlaidItem>();
            }
            return plaidItems;
        }
        public async Task<PlaidItem?> GetPlaidItem(string profileId, string itemId)
        {
            var plaidItem = await _dynamoDBContext.LoadAsync<PlaidItem>(profileId, itemId);
            if (plaidItem == null)
            {
                Console.WriteLine($"No Plaid item found for profileId: {profileId} and itemId: {itemId}");
                return null;  
            }
            return plaidItem;
        }
        public async Task AddPlaidItem(string accessToken, string institutionId, string institution, string itemId, string profileId)
        {
            var plaidItem = new PlaidItem
            {
                profileId = profileId,
                itemId = itemId,
                accessToken = accessToken,
                institutionId = institutionId,
                institution = institution,
                createdAt = DateTime.UtcNow.ToString("o")
            };

            await _dynamoDBContext.SaveAsync(plaidItem);    

        }
        public async Task<string?> GetCursor(string profileId, string itemId)
        {
            var plaidItem = await _dynamoDBContext.LoadAsync<PlaidItem>(profileId, itemId);
            
            if (plaidItem == null)
                Console.WriteLine($"No Plaid item found for profileId: {profileId}, itemId: {itemId}");
            
            return plaidItem?.transactionCursor;
        }
        public async Task UpdateCursor(string profileId, string itemId, string? newCursor)
        {
            var plaidItem = await _dynamoDBContext.LoadAsync<PlaidItem>(profileId, itemId);
            if (plaidItem == null)
            {
                Console.WriteLine($"No Plaid item found for profileId: {profileId}, itemId: {itemId}");
                return;  
            }
            plaidItem.transactionCursor = newCursor;
            await _dynamoDBContext.SaveAsync(plaidItem);
        }
    }
}