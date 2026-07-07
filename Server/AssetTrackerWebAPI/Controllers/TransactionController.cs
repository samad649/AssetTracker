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
    }

}