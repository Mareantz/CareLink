using Application;
using Application.Utils;
using Infrastructure;
using Identity;
var builder = WebApplication.CreateBuilder(args);
//builder.Configuration.AddEnvironmentVariables();
//Console.WriteLine($"Connection String #1: {builder.Configuration.GetConnectionString("DefaultConnection")}");
//var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
//builder.WebHost.UseUrls($"http://*:{port}");

var AllowFrontend = "AllowFrontend";

builder.Services.AddCors(options =>
{
	options.AddPolicy(name: AllowFrontend,
		policy =>
		{
			policy.WithOrigins("http://loocalhost:8080");
			policy.AllowAnyHeader();
				policy.AllowAnyMethod();
		});

});

//Console.WriteLine("Environment Variables:");
//foreach (var variable in Environment.GetEnvironmentVariables().Keys)
//{
//	Console.WriteLine($"{variable}: {Environment.GetEnvironmentVariable(variable.ToString())}");
//}
//builder.Configuration["ConnectionStrings:DefaultConnection"] = Environment.GetEnvironmentVariable("DefaultConnection");
//Console.WriteLine($"Connection String #2: {builder.Configuration.GetConnectionString("DefaultConnection")}");


//builder.Services.AddHealthChecks();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddIdentity(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//app.UseHealthChecks("/health");

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseCors(AllowFrontend);
app.UseRouting();
app.UseStaticFiles();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();
<<<<<<< Updated upstream
app.RunAsync();
=======
app.Run();
>>>>>>> Stashed changes
