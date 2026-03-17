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


       public ProfileController(MockDataService mockDataService)
       {
        _mockDataService = mockDataService;
       } 
         [HttpPost("CreateMockProfile")]
        public async Task<ActionResult<Profile>> CreateMockProfile()
        {
            var createdProfile = await _mockDataService.CreateMockProfile();
            return Ok(createdProfile);
        }
        [HttpGet("{profileId}")]
        public 
    }
    
}