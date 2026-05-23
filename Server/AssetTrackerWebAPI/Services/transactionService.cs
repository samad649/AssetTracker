using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
namespace AssetTrackerWebAPI.Services
{
  public class transactionService{
        private readonly IDynamoDBContext _dynamoDBContext;

        public transactionService(IDynamoDBContext dynamoDBContext)
        {
            _dynamoDBContext = dynamoDBContext;
        }
    }  
}