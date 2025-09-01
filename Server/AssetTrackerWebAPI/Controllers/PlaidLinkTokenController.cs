using Microsoft.AspNetCore.Mvc;
using Going.Plaid;
using Going.Plaid.Entity;
using Going.Plaid.Link;
using DotNetEnv;

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

    }
}