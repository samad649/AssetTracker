using Amazon.DynamoDBv2.DataModel;

namespace AssetTrackerWebAPI.Services
{
    public class ProfileService
    {
        private readonly IDynamoDBContext _dynamoDBContext;
        private readonly UserService _userService;

        public ProfileService(IDynamoDBContext dynamoDBContext, UserService userService)
        {
            _dynamoDBContext = dynamoDBContext;
            _userService = userService;
        }

        public async Task<Profile?> GetProfileByProfileId(string profileId)
        {
            return await _dynamoDBContext.LoadAsync<Profile>(profileId);
        }

        public async Task<Profile?> GetProfileByUserId(string userId)
        {
            var user = await _userService.GetUser(userId);
            if (user == null) return null;  // ← return null not NotFound()

            return await _dynamoDBContext.LoadAsync<Profile>(user.profileId);
        }
        public async Task CreateProfile(Profile profile)
        {
            await _dynamoDBContext.SaveAsync(profile);
        }
    }
}