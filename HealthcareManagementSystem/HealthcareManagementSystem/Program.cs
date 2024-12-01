using Application;
using Application.Utils;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
Console.WriteLine($"Connection String: {builder.Configuration.GetConnectionString("DefaultConnection")}");
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

var AllowFrontend = "AllowFrontend";

builder.Services.AddCors(options =>
{
	options.AddPolicy(name: AllowFrontend,
		builder =>
		{
			builder.WithOrigins("https://healthcaremanagement-fe.vercel.app")
				.AllowAnyHeader()
				.AllowAnyMethod();
		});

});

Console.WriteLine("Environment Variables:");
foreach (var variable in Environment.GetEnvironmentVariables().Keys)
{
	Console.WriteLine($"{variable}: {Environment.GetEnvironmentVariable(variable.ToString())}");
}
builder.Configuration["ConnectionStrings:DefaultConnection"] = Environment.GetEnvironmentVariable("DefaultConnection");


builder.Services.AddHealthChecks();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHealthChecks("/health");

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseRouting();
app.UseStaticFiles();
app.MapControllers();
await app.RunAsync();