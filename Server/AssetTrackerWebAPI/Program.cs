using DotNetEnv;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AssetTrackerWebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Going.Plaid;
using Microsoft.OpenApi.Models;

var dynamoConfig = new AmazonDynamoDBConfig
{
    ServiceURL = "http://localhost:8000",
    AuthenticationRegion = "us-east-1"
};

var dynamoClient = new AmazonDynamoDBClient("dummy", "dummy", dynamoConfig);

// Create a builder for the web application
var builder = WebApplication.CreateBuilder(args);
// Enable API Explorer
builder.Services.AddEndpointsApiExplorer();
// Enable Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AssetTracker API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
// Enable controllers
builder.Services.AddControllers();
//Add DynamoDB services to the dependency injection container
builder.Services.AddSingleton<IAmazonDynamoDB>(dynamoClient);
builder.Services.AddSingleton<IDynamoDBContext>(sp =>
    new DynamoDBContextBuilder()
        .WithDynamoDBClient(() => sp.GetRequiredService<IAmazonDynamoDB>())
        .Build());
builder.Services.AddPlaid(builder.Configuration);  
builder.Services.AddScoped<accountService>();
builder.Services.AddScoped<transactionService>();
builder.Services.AddScoped<plaidService>();   
builder.Services.AddScoped<plaidItemService>();
builder.Services.AddScoped<MockDataService>();      
builder.Services.AddScoped<ProfileService>();    
builder.Services.AddScoped<AuthService>();    
builder.Services.AddScoped<UserService>();
builder.Services.AddHostedService<DBinitService>();

var secretKey = builder.Configuration["Jwt:SecretKey"] 
    ?? throw new Exception("JWT SecretKey not configured");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey) // ← use variable here
            )
        };
    });
builder.Services.AddAuthorization();
// Build the app
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
}
app.UseHttpsRedirection();
// Configure CORS to allow requests from Angular dev server
app.UseCors(builder =>
    builder
        .WithOrigins("http://localhost:4200") // Angular dev server
        .AllowAnyHeader()
        .AllowAnyMethod()
);
// Allows use of authentication for endpoints
app.UseAuthentication();
// Allows use of authorization for endpoints
app.UseAuthorization();
// Map controller routes
app.MapControllers();
// Run the application  
app.Run();

