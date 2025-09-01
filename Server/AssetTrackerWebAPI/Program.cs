//
var builder = WebApplication.CreateBuilder(args);
// Enable API Explorer
builder.Services.AddEndpointsApiExplorer();
// Enable Swagger
builder.Services.AddSwaggerGen();
// Enable controllers
builder.Services.AddControllers();
// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Maps HTTP requests to HTTPS
app.UseHttpsRedirection();
// Allows use of authorization for endpoints
app.UseAuthorization();
// Map controller routes
app.MapControllers();

app.Run();

