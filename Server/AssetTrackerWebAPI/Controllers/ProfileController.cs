using Microsoft.AspNetCore.Mvc;
using AssetTrackerWebAPI.Services;
using Amazon.DynamoDBv2.DataModel;
namespace AssetTrackerWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController: ControllerBase
    {
       private readonly MockDataService _mockDataService;
       private readonly IDynamoDBContext _dynamoDBContext;

       public ProfileController(MockDataService mockDataService, IDynamoDBContext dynamoDBContext)
       {
        _mockDataService = mockDataService;
        _dynamoDBContext = dynamoDBContext;
       } 
         [HttpPost("CreateMockProfile")]
        public async Task<ActionResult<Profile>> CreateMockProfile()
        {
            var createdProfile = _mockDataService.CreateMockProfile();
            await _dynamoDBContext.SaveAsync(createdProfile);
            return Ok(createdProfile);
        }
    }
    
}