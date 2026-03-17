namespace AssetTrackerWebAPI.Services
{
    public class MockDataService
    {
        private static readonly Random _random = new Random();

        public MockDataService()
        {

        }
        public Transaction GetMockTransaction(string accountId)
        {
            var vendors = new List<string> { "Amazon", "Walmart", "Starbucks", "Apple", "Netflix","Target", "Uber", "Airbnb", 
            "Spotify", "Best Buy", "Costco", "Home Depot", "Macy's", "Nike", "Adidas", "CVS", "Daves Hot Chicken", "Aga's", "Chick-fil-A", "Chipotle"};
            
            return new Transaction
            {
                transactionId = Guid.NewGuid().ToString(),
                accountId = accountId,
                vendor = vendors[_random.Next(0, vendors.Count)],
                amount = (float)Math.Round(_random.NextDouble() * 100, 2),
                date = DateTime.UtcNow.AddDays(-_random.Next(0, 30))
            };
        }
        public Account GetMockAccount(string profileId)
        {
            var types = new[] { "Checking", "Savings", "Investment", "Credit", "Crypto" };

            var institutions = new List<string> { "Chase", "SoFi", "Bank of America", "Wells Fargo", "Citibank", "Capital One", "PNC Bank", "TD Bank", "Robinhood", "Coinbase",
             "Ally", "US Bank", "Charles Schwab", "Fidelity", "Vanguard", "American Express", "Discover", "Goldman Sachs", "Morgan Stanley", "Barclays", "HSBC" };

            return new Account
            {
                profileId = profileId,
                accountId = Guid.NewGuid().ToString(),
                balance = (float)Math.Round(_random.NextDouble() * 10000, 2),
                type = types[_random.Next(0,types.Length)],
                institution = institutions[_random.Next(0, institutions.Count)]
            };
        }
        public Profile GetMockProfile()
        {
            var firstNames = new List<string> { "John", "Jane", "Michael", "Emily", "David", "Sarah", "Robert", "Jessica", "Daniel", "Laura",
             "James", "Olivia", "William", "Sophia", "Joseph", "Isabella", "Charles", "Mia", "Thomas", "Amelia" };
            var lastNames = new List<string> { "Smith", "Johnson", "Brown", "Taylor", "Anderson", "Thomas", "Jackson", "White", "Harris", "Martin",
             "Thompson", "Garcia", "Martinez", "Robinson" };
            string firstName = firstNames[_random.Next(0, firstNames.Count)];
            string lastName = lastNames[_random.Next(0, lastNames.Count)];
            string email = $"{firstName}.{lastName}@gmail.com";
            string profileId = Guid.NewGuid().ToString();
            int numAccounts = _random.Next(1, 4);
            var accountIds = new List<string>();
            for (int i = 0; i < numAccounts; i++)
            {
                var account = GetMockAccount(profileId);
                accountIds.Add(account.accountId);
            }
            return new Profile
            {
                profileId = profileId,
                firstName = firstName,
                lastName = lastName,
                email = email,
                accountIds = accountIds
            };
        }

    }
}