using Going.Plaid;
//
var builder = WebApplication.CreateBuilder(args);
// Enable API Explorer
builder.Services.AddEndpointsApiExplorer();
// Enable Swagger
builder.Services.AddSwaggerGen();
// Enable controllers
builder.Services.AddControllers();

//define the Plaid Client to interact with API
builder.Services.Configure<PlaidOptions>(options =>
{
    options.ClientId = "#";
    options.Secret = "#";
    options.Environment = Going.Plaid.Environment.Sandbox;
});

builder.Services.AddSingleton<PlaidClient>();
// Build the app
var app = builder.Build();

app.UseCors(builder =>
    builder
        .WithOrigins("http://localhost:4200") // Angular dev server
        .AllowAnyHeader()
        .AllowAnyMethod()
);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Maps HTTP requests to HTTPS
// app.UseHttpsRedirection();
// Allows use of authorization for endpoints
app.UseAuthorization();
// Map controller routes
app.MapControllers();

app.Run();

