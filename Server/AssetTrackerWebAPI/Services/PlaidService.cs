using Going.Plaid;
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
        public 
        //fetch and store access token and item id from database
        //get transactions using access token
        //store transactions in database
        //get bank info and balances using access token
        public string getAccessToken(string itemId)
        {
            //fetch access token from database
            return "access_token";
        }

    }
}