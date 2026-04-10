using Amazon.DynamoDBv2.DataModel;

namespace AssetTrackerWebAPI.Services
{
    public class MockDataService
    {
        private static readonly Random _random = new Random();
        private readonly IDynamoDBContext _dynamoDBContext;

        public MockDataService(IDynamoDBContext dynamoDBContext)
        {
            _dynamoDBContext = dynamoDBContext;
        }
        public async Task CreateMockTransaction(string accountId)
        {
            var vendors = new List<string> { "Amazon", "Walmart", "Starbucks", "Apple", "Netflix","Target", "Uber", "Airbnb", 
            "Spotify", "Best Buy", "Costco", "Home Depot", "Macy's", "Nike", "Adidas", "CVS", "Daves Hot Chicken", "Aga's", "Chick-fil-A", "Chipotle"};
            
            var transaction = new Transaction
            {
            transactionId = Guid.NewGuid().ToString(),
            accountId = accountId,
            vendor = vendors[_random.Next(0, vendors.Count)],
            amount = (float)Math.Round(_random.NextDouble() * 100, 2),
            date = DateTime.UtcNow.AddDays(-_random.Next(0, 30))
            };

            await _dynamoDBContext.SaveAsync(transaction);

        }
        public async Task CreateMockAccount(string profileId)
        {
            var types = new[] { "Checking", "Savings", "Investment", "Credit", "Crypto" };

            var institutions = new List<string> { "Chase", "SoFi", "Bank of America", "Wells Fargo", "Citibank", "Capital One", "PNC Bank", "TD Bank", "Robinhood", "Coinbase",
             "Ally", "US Bank", "Charles Schwab", "Fidelity", "Vanguard", "American Express", "Discover", "Goldman Sachs", "Morgan Stanley", "Barclays", "HSBC" };

            var account =  new Account
            {
                profileId = profileId,
                accountId = Guid.NewGuid().ToString(),
                balance = (float)Math.Round(_random.NextDouble() * 10000, 2),
                type = types[_random.Next(0,types.Length)],
                institution = institutions[_random.Next(0, institutions.Count)]
            };

            int numAccounts = _random.Next(5, 50);
            for (int i = 0; i < numAccounts; i++)
            {
                await CreateMockTransaction(account.accountId);
            }

            await _dynamoDBContext.SaveAsync(account);

        }
        public async Task<Profile> CreateMockProfile()
        {
            
            var firstNames = new List<string> { "John", "Jane", "Michael", "Emily", "David", "Sarah", "Robert", "Jessica", "Daniel", "Laura",
             "James", "Olivia", "William", "Sophia", "Joseph", "Isabella", "Charles", "Mia", "Thomas", "Amelia" };
            var lastNames = new List<string> { "Smith", "Johnson", "Brown", "Taylor", "Anderson", "Thomas", "Jackson", "White", "Harris", "Martin",
             "Thompson", "Garcia", "Martinez", "Robinson" };
            string firstName = firstNames[_random.Next(0, firstNames.Count)];
            string lastName = lastNames[_random.Next(0, lastNames.Count)];
            string profileId = Guid.NewGuid().ToString();
            int numAccounts = _random.Next(1, 6);
            for (int i = 0; i < numAccounts; i++)
            {
                await CreateMockAccount(profileId);
            }
            var profile = new Profile
            {
                profileId = profileId,
                firstName = firstName,
                lastName = lastName,
            };
            await _dynamoDBContext.SaveAsync(profile);

            return profile;
        }
        public async Task<User> CreateMockUser()
        {
            var profile = await CreateMockProfile();

            string email = $"{profile.firstName}.{profile.lastName}@gmail.com";

            var user = new User
            {
                userId = Guid.NewGuid().ToString(),
                profileId = profile.profileId,
                email = email,
                password = "password123",
                username = $"{profile.firstName}-{profile.lastName}-{_random.Next(1000,9999)}"
            };

            await _dynamoDBContext.SaveAsync(user);
            return user;
        }

    }
}