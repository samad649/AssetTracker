using Amazon.DynamoDBv2.DataModel;
namespace AssetTrackerWebAPI.Services
{
    public class ProfileService
    {
        private readonly IDynamoDBContext _dynamoDBContext;
        private readonly MockDataService _mockDataService;

        public ProfileService(IDynamoDBContext dynamoDBContext, MockDataService mockDataService)
        {
            _dynamoDBContext = dynamoDBContext;
            _mockDataService = mockDataService;
        }
        public async Task CreateProfileAsync(Profile profile)
        {
            profile = _mockDataService.GetMockProfile();
            await _dynamoDBContext.SaveAsync(profile);
        }
    }
}