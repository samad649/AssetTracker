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
        public async Task<Profile> CreateMockProfile()
        {  
            var firstNames = new List<string> { "John", "Jane", "Michael", "Emily", "David", "Sarah", "Robert", "Jessica", "Daniel", "Laura",
             "James", "Olivia", "William", "Sophia", "Joseph", "Isabella", "Charles", "Mia", "Thomas", "Amelia" };
            var lastNames = new List<string> { "Smith", "Johnson", "Brown", "Taylor", "Anderson", "Thomas", "Jackson", "White", "Harris", "Martin",
             "Thompson", "Garcia", "Martinez", "Robinson" };
            string firstName = firstNames[_random.Next(0, firstNames.Count)];
            string lastName = lastNames[_random.Next(0, lastNames.Count)];
            string profileId = Guid.NewGuid().ToString();
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