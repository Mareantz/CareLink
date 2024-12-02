using Application;
using Application.Utils;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add environment variables
builder.Configuration.AddEnvironmentVariables();

// Log environment variables and connection string for debugging
Console.WriteLine($"Connection String from Environment Variable: {Environment.GetEnvironmentVariable("DefaultConnection")}");
Console.WriteLine($"Connection String from Configuration: {builder.Configuration.GetConnectionString("DefaultConnection")}");
Console.WriteLine("Environment Variables:");
foreach (var variable in Environment.GetEnvironmentVariables().Keys)
{
	Console.WriteLine($"{variable}: {Environment.GetEnvironmentVariable(variable.ToString())}");
}

// Explicitly set the connection string in the configuration if needed
builder.Configuration["ConnectionStrings:DefaultConnection"] = Environment.GetEnvironmentVariable("DefaultConnection");

// Ensure the connection string is available
if (string.IsNullOrEmpty(builder.Configuration.GetConnectionString("DefaultConnection")))
{
	throw new InvalidOperationException("DefaultConnection string is not properly initialized.");
}

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

// Define CORS policy
var AllowFrontend = "AllowFrontend";

builder.Services.AddCors(options =>
{
	options.AddPolicy(name: AllowFrontend, policy =>
	{
		policy.WithOrigins("https://healthcaremanagement-fe.vercel.app")
			.AllowAnyHeader()
			.AllowAnyMethod();
	});
});

// Register services
builder.Services.AddHealthChecks();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the app pipeline
app.UseHealthChecks("/health");

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(AllowFrontend);
app.UseRouting();
app.UseStaticFiles();
app.MapControllers();

await app.RunAsync();
