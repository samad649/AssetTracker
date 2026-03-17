using DotNetEnv;
using Going.Plaid;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AssetTrackerWebAPI.Services;



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
builder.Services.AddSwaggerGen();
// Enable controllers
builder.Services.AddControllers();
//Add DynamoDB services to the dependency injection container
builder.Services.AddSingleton<IAmazonDynamoDB>(dynamoClient);
builder.Services.AddSingleton<IDynamoDBContext>(sp =>
    new DynamoDBContextBuilder()
        .WithDynamoDBClient(() => sp.GetRequiredService<IAmazonDynamoDB>())
        .Build());
builder.Services.AddSingleton<ProfileService>(); 
builder.Services.AddScoped<MockDataService>();
builder.Services.AddHostedService<DBinitService>();
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

