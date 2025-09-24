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
        //fetch and store access token and item id from database
        
        //store transactions in database
        public string getAccessToken(string itemId)
        {
            //fetch access token from database
            return "access_token";
        }
        //get transactions using access token
        public getTransactionData(string accessToken, DateTime startDate, DateTime endDate)
        { }
        //get bank info and balances using access token
        public getBankInfoAndBalances(string accessToken)
        { }
        //get identity info using access token
        public getIdentityInfo(string accessToken)
        { }
        
            
    }
}