using Microsoft.AspNetCore.Mvc;
using Going.Plaid;
using Going.Plaid.Entity;
using Going.Plaid.Link;
using DotNetEnv;
using Going.Plaid.Item;
using AssetTrackerWebAPI.Models;
namespace AssetTrackerWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlaidLinkTokenController : ControllerBase
    {
        private readonly PlaidClient _plaidClient;

        public PlaidLinkTokenController(PlaidClient plaidClient)
        {
            Env.Load();
            _plaidClient = plaidClient;
        }

        [HttpGet("create")]
        public async Task<IActionResult> CreateLinkToken()
        {
            string client_id = Env.GetString("PLAID_CLIENT_ID");
            if (client_id != null)
            {
                var request = new LinkTokenCreateRequest
                {
                    User = new LinkTokenCreateRequestUser { ClientUserId = "Samad649" },
                    ClientName = "Asset Tracker",
                    Products = new List<Products> { Products.Auth },
                    CountryCodes = new List<CountryCode> { CountryCode.Us, CountryCode.Ca },
                    Language = Language.English
                };
                var response = await _plaidClient.LinkTokenCreateAsync(request);
                return Ok(new { link_token = response.LinkToken });
            }
            else
            {
                return BadRequest(new { error = "Client ID is not set." });
            }


        }
        [HttpPost("exchange")]
        public async Task<IActionResult> ExchangePublicToken([FromBody] PublicToken data)
        {
            Console.WriteLine("Received data: " + data);
              try
            {
                string publicToken = data.public_token;
                var request = new ItemPublicTokenExchangeRequest
                {
                    PublicToken = publicToken
                };
                var response = await _plaidClient.ItemPublicTokenExchangeAsync(request);
                string accessToken = response.AccessToken;
                string itemId = response.ItemId;
                Console.WriteLine("Access Token: " + accessToken);
                Console.WriteLine("Item ID: " + itemId);
                // Store the accessToken and itemId securely in DB (create Service for storing)
                return Ok(new { access_token = accessToken, item_id = itemId });
            }
            catch (Exception ex)
            {
                // Log the full exception to the console
                Console.WriteLine("Exception: " + ex.ToString());
                // Return the error message to the client for debugging (remove in production)
                return StatusCode(500, new { error = ex.Message });
            }
        }

    }
}