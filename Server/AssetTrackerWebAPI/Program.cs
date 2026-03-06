using DotNetEnv;
using Going.Plaid;
using Amazon.DynamoDBv2;

Env.Load();
// Create a builder for the web application
var builder = WebApplication.CreateBuilder(args);
// Enable API Explorer
builder.Services.AddEndpointsApiExplorer();
// Enable Swagger
builder.Services.AddSwaggerGen();
// Enable controllers
builder.Services.AddControllers();
//define AWS DynamoDB service
builder.Services.AddAWSService<IAmazonDynamoDB>();
//define the Plaid Client to interact with API
builder.Services.Configure<PlaidOptions>(options =>
{
    options.ClientId =  Env.GetString("PLAID_CLIENT_ID") ?? "";
    options.Secret = Env.GetString("PLAID_SECRET") ?? "";
    options.Environment = Going.Plaid.Environment.Sandbox;
});

builder.Services.AddSingleton<PlaidClient>();
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

