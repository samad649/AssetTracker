using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace AssetTrackerWebAPI.Services
{
    public class AuthService
    {
        private readonly IConfiguration _config;
        private readonly IDynamoDBContext _dynamoDBContext;

        public AuthService(IConfiguration config, IDynamoDBContext dynamoDBContext)
        {
            _config = config;
            _dynamoDBContext = dynamoDBContext;
        }

        public string GenerateToken(User user)
        {
            var secretKey = _config["Jwt:SecretKey"] ?? throw new Exception("JWT secret key not configured");
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var token = new JwtSecurityToken(
                claims: new[] {
                    new Claim("userId", user.userId ?? string.Empty),
                    new Claim("email", user.email ?? string.Empty),
                    new Claim("profileId", user.profileId ?? string.Empty)
                },
                expires: DateTime.Now.AddHours(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<User?> Validate(string username, string password)
        {
            var query = _dynamoDBContext.QueryAsync<User>(
                username,
                new QueryConfig
                {
                    IndexName = "username-index"
                }
            );

            var users = await query.GetRemainingAsync();
            var user = users.FirstOrDefault();

            if (user == null) return null;
            if (user.password != password) return null;

            return user;
        }
    }
}