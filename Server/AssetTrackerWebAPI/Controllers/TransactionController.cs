using Microsoft.AspNetCore.Mvc;
using AssetTrackerWebAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace AssetTrackerWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]    
    public class TransactionController: ControllerBase
    {
        private readonly plaidService _plaidService;
        private readonly plaidItemService _plaidItemService;
        private readonly transactionService _transactionService;

        public TransactionController(plaidService plaidService, plaidItemService plaidItemService, transactionService transactionService)
        {
            _plaidService = plaidService;
            _plaidItemService = plaidItemService;
            _transactionService = transactionService;
        }
        [HttpGet("get/{profileId}")]
        public async Task<IActionResult> GetTransactionsByProfile(string profileId)
        {
            var transactions = await _transactionService.GetTransactionsByProfile(profileId);
            return Ok(transactions);
        }
        [HttpGet("get/{profileId}/{accountId}")]
        public async Task<IActionResult> GetTransactionsByAccount(string accountId)
        {
            var transactions = await _transactionService.GetTransactionsByAccount(accountId);
            return Ok(transactions);
        }
        [HttpPost("sync/{profileId}")]
        public async Task<IActionResult> SyncAllTransactions(string profileId)
        {
            var items = await _plaidItemService.GetItemsByProfile(profileId);
            foreach (var item in items)
            {
                    if (item.accessToken == null)
                        {
                            Console.WriteLine($"No access token for itemId: {item.itemId}");
                            continue;
                        }
                try 
                    {await _plaidService.storeTransactionData(item.accessToken, item.itemId, profileId);}
                catch (Exception ex)
                    {Console.WriteLine($"Failed to sync itemId: {item.itemId} - {ex.Message}");}           
            }
            return Ok("Successfully synced transactions for all items");
        }
        
        [HttpPost("sync/{profileId}/{itemId}")]
        public async Task<IActionResult> SyncItemTransactions(string profileId, string itemId)
        {
            var item = await _plaidItemService.GetPlaidItem(profileId, itemId);
            if (item == null)
            {
                return NotFound($"No Plaid item found for profileId: {profileId} and itemId: {itemId}");
            }
            await _plaidService.storeTransactionData(item.accessToken, item.itemId, profileId);
            return Ok("Successfully synced transactions for the specified item");
        }
    }

}